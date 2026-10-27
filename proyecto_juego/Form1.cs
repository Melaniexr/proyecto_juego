using System;
using System.Drawing;
using System.Windows.Forms;

namespace proyecto_juego
{
    public partial class Form1 : Form
    {
        Random aleatorio = new Random();

        string[] nombres = new string[2];
        int[,] estadisticas = new int[2, 3]; // Aciertos, errores, puntos
        int[] preguntasHechas = new int[2];

        string[] historialNombres = new string[100];
        int[] historialPuntos = new int[100];
        int cantidadHistorial = 0;

        int cantidadJugadores = 1;
        int turno = 0;
        int respuestaCorrecta = 0;

        Panel pantallaInicio = new Panel();
        Panel pantallaJuego = new Panel();
        Panel pantallaResultados = new Panel();

        ComboBox cmbJugadores = new ComboBox();
        TextBox txtNombre1 = new TextBox();
        TextBox txtNombre2 = new TextBox();
        Label lblNombre2 = new Label();

        Label lblTurno = new Label();
        Label lblPregunta = new Label();
        Label lblPuntos = new Label();
        Label lblAviso = new Label();
        TextBox txtRespuesta = new TextBox();

        Label lblTituloResultados = new Label();
        Label lblResumen = new Label();
        TextBox txtRanking = new TextBox();

        public Form1()
        {
            InitializeComponent();
            CrearPantallas();
            MostrarPantalla(pantallaInicio);
        }

        private Label CrearEtiqueta(Panel panel, string texto, int x, int y,
                                   int ancho, int alto, int tamano)
        {
            Label etiqueta = new Label();
            etiqueta.Text = texto;
            etiqueta.Location = new Point(x, y);
            etiqueta.Size = new Size(ancho, alto);
            etiqueta.Font = new Font("Segoe UI", tamano);
            panel.Controls.Add(etiqueta);
            return etiqueta;
        }

        private void CrearBoton(Panel panel, string texto, int x, int y,
                               int ancho, EventHandler accion)
        {
            Button boton = new Button();
            boton.Text = texto;
            boton.Location = new Point(x, y);
            boton.Size = new Size(ancho, 42);
            boton.Font = new Font("Segoe UI", 11);
            boton.Click += accion;
            panel.Controls.Add(boton);
        }

        private void CrearPantallas()
        {
            Text = "Math Cat - Juego Matemático";
            ClientSize = new Size(780, 590);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.White;

            pantallaInicio.Dock = DockStyle.Fill;
            pantallaJuego.Dock = DockStyle.Fill;
            pantallaResultados.Dock = DockStyle.Fill;
            pantallaInicio.BackColor = Color.White;
            pantallaJuego.BackColor = Color.White;
            pantallaResultados.BackColor = Color.White;
            Controls.Add(pantallaInicio);
            Controls.Add(pantallaJuego);
            Controls.Add(pantallaResultados);

            // PANTALLA DE INICIO
            // Titulo y gatito al lado.
            Label titulo = CrearEtiqueta(pantallaInicio, "MATH CAT", 230, 45, 285, 60, 25);
            titulo.TextAlign = ContentAlignment.MiddleCenter;

            Label gatito = CrearEtiqueta(pantallaInicio, "🐱", 525, 45, 65, 60, 28);
            gatito.Font = new Font("Segoe UI Emoji", 28);
            gatito.TextAlign = ContentAlignment.MiddleLeft;

            Label subtitulo = CrearEtiqueta(pantallaInicio, "Desafío matemático", 190, 105, 400, 35, 12);
            subtitulo.TextAlign = ContentAlignment.MiddleCenter;
            CrearEtiqueta(pantallaInicio, "Modo de juego:", 175, 175, 160, 30, 12);

            cmbJugadores.Location = new Point(345, 174);
            cmbJugadores.Size = new Size(240, 30);
            cmbJugadores.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbJugadores.Items.Add("1 jugador");
            cmbJugadores.Items.Add("2 jugadores");
            cmbJugadores.SelectedIndex = 0;
            cmbJugadores.SelectedIndexChanged += CambiarModo;
            pantallaInicio.Controls.Add(cmbJugadores);

            CrearEtiqueta(pantallaInicio, "Jugador 1:", 175, 237, 160, 30, 12);
            txtNombre1.Location = new Point(345, 237);
            txtNombre1.Size = new Size(240, 30);
            pantallaInicio.Controls.Add(txtNombre1);

            lblNombre2 = CrearEtiqueta(pantallaInicio, "Jugador 2:", 175, 288, 160, 30, 12);
            txtNombre2.Location = new Point(345, 288);
            txtNombre2.Size = new Size(240, 30);
            pantallaInicio.Controls.Add(txtNombre2);
            lblNombre2.Visible = false;
            txtNombre2.Visible = false;

            CrearEtiqueta(pantallaInicio, "5 puntos por acierto | Maximo 3 errores | 10 preguntas por jugador",
                          135, 365, 580, 35, 11);
            CrearBoton(pantallaInicio, "Iniciar juego", 225, 430, 155, IniciarJuego);
            CrearBoton(pantallaInicio, "Ver ranking", 400, 430, 155, VerRanking);

            // PANTALLA DE JUEGO
            CrearEtiqueta(pantallaJuego, "RESUELVA LA OPERACION", 210, 45, 460, 50, 23);
            lblTurno = CrearEtiqueta(pantallaJuego, "", 110, 115, 600, 40, 14);
            lblPregunta = CrearEtiqueta(pantallaJuego, "", 215, 182, 430, 70, 27);
            CrearEtiqueta(pantallaJuego, "Respuesta:", 205, 270, 145, 34, 13);
            txtRespuesta.Location = new Point(360, 266);
            txtRespuesta.Size = new Size(180, 32);
            txtRespuesta.Font = new Font("Segoe UI", 14);
            txtRespuesta.KeyDown += RespuestaEnter;
            pantallaJuego.Controls.Add(txtRespuesta);
            CrearBoton(pantallaJuego, "Responder", 300, 326, 180, ComprobarRespuesta);
            lblAviso = CrearEtiqueta(pantallaJuego, "", 110, 393, 590, 38, 12);
            lblPuntos = CrearEtiqueta(pantallaJuego, "", 110, 443, 610, 60, 12);
            CrearBoton(pantallaJuego, "Salir de la partida", 290, 520, 200, SalirDelJuego);

            // PANTALLA DE RESULTADOS Y ESTADISTICAS
            lblTituloResultados = CrearEtiqueta(pantallaResultados, "RESULTADOS", 220, 35, 480, 50, 24);
            lblResumen = CrearEtiqueta(pantallaResultados, "", 90, 102, 610, 140, 12);
            CrearEtiqueta(pantallaResultados, "Ranking de esta sesion:", 90, 258, 440, 33, 15);
            txtRanking.Location = new Point(90, 298);
            txtRanking.Size = new Size(595, 205);
            txtRanking.Multiline = true;
            txtRanking.ReadOnly = true;
            txtRanking.ScrollBars = ScrollBars.Vertical;
            txtRanking.Font = new Font("Consolas", 11);
            pantallaResultados.Controls.Add(txtRanking);
            CrearBoton(pantallaResultados, "Volver al inicio", 290, 525, 200, VolverAlInicio);
        }

        private void MostrarPantalla(Panel pantalla)
        {
            pantallaInicio.Visible = false;
            pantallaJuego.Visible = false;
            pantallaResultados.Visible = false;
            pantalla.Visible = true;
            pantalla.BringToFront();
        }

        private void CambiarModo(object sender, EventArgs e)
        {
            bool dosJugadores = cmbJugadores.SelectedIndex == 1;
            lblNombre2.Visible = dosJugadores;
            txtNombre2.Visible = dosJugadores;
        }

        private void IniciarJuego(object sender, EventArgs e)
        {
            if (txtNombre1.Text.Trim() == "")
            {
                MessageBox.Show("Escriba el nombre del jugador 1.");
                return;
            }

            cantidadJugadores = cmbJugadores.SelectedIndex + 1;
            if (cantidadJugadores == 2 && txtNombre2.Text.Trim() == "")
            {
                MessageBox.Show("Escriba el nombre del jugador 2.");
                return;
            }

            nombres[0] = txtNombre1.Text.Trim();
            nombres[1] = txtNombre2.Text.Trim();
            for (int fila = 0; fila < 2; fila++)
            {
                preguntasHechas[fila] = 0;
                for (int columna = 0; columna < 3; columna++)
                {
                    estadisticas[fila, columna] = 0;
                }
            }

            turno = 0;
            lblAviso.Text = "";
            MostrarPantalla(pantallaJuego);
            GenerarPregunta();
        }

        private bool PuedeSeguir(int jugador)
        {
            if (jugador >= cantidadJugadores) return false;
            if (estadisticas[jugador, 1] >= 3) return false;
            if (preguntasHechas[jugador] >= 10) return false;
            return true;
        }

        private void GenerarPregunta()
        {
            int aciertosTotales = estadisticas[0, 0] + estadisticas[1, 0];
            int nivel = 1;
            if (aciertosTotales >= 3) nivel = 2;

            int maximo = 11;
            int opciones = 2; // Nivel 1: suma y resta
            if (nivel == 2)
            {
                maximo = 101;
                opciones = 4; // Nivel 2: las cuatro operaciones
            }

            int numero1 = aleatorio.Next(1, maximo);
            int numero2 = aleatorio.Next(1, maximo);
            int operacion = aleatorio.Next(0, opciones);
            string signo = "+";

            if (operacion == 0)
            {
                respuestaCorrecta = numero1 + numero2;
                signo = "+";
            }
            else if (operacion == 1)
            {
                if (numero2 > numero1)
                {
                    int temporal = numero1;
                    numero1 = numero2;
                    numero2 = temporal;
                }
                respuestaCorrecta = numero1 - numero2;
                signo = "-";
            }
            else if (operacion == 2)
            {
                numero2 = aleatorio.Next(1, 11);
                respuestaCorrecta = numero1 * numero2;
                signo = "x";
            }
            else if (operacion == 3)
            {
                numero2 = aleatorio.Next(1, 11);
                int cociente = aleatorio.Next(1, 11);
                numero1 = numero2 * cociente; // Division exacta
                respuestaCorrecta = cociente;
                signo = "/";
            }

            lblTurno.Text = "Turno: " + nombres[turno] + "  |  Nivel: " + nivel +
                            "  |  Pregunta: " + (preguntasHechas[turno] + 1) + " de 10";
            lblPregunta.Text = numero1 + " " + signo + " " + numero2 + " = ?";
            txtRespuesta.Text = "";
            ActualizarPuntos();
            txtRespuesta.Focus();
        }

        private void ActualizarPuntos()
        {
            string texto = "";
            for (int i = 0; i < cantidadJugadores; i++)
            {
                texto += nombres[i] + ": " + estadisticas[i, 2] + " puntos, " +
                         estadisticas[i, 0] + " aciertos, " + estadisticas[i, 1] +
                         " errores.  ";
            }
            lblPuntos.Text = texto;
        }

        private void RespuestaEnter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ComprobarRespuesta(sender, e);
                e.SuppressKeyPress = true;
            }
        }

        private void ComprobarRespuesta(object sender, EventArgs e)
        {
            int respuesta;
            if (!int.TryParse(txtRespuesta.Text.Trim(), out respuesta))
            {
                MessageBox.Show("Ingrese una respuesta numerica valida.");
                return;
            }

            preguntasHechas[turno]++;
            string aviso;
            if (respuesta == respuestaCorrecta)
            {
                estadisticas[turno, 0]++;
                estadisticas[turno, 2] += 5;
                aviso = "Respuesta correcta: +5 puntos.";
            }
            else
            {
                estadisticas[turno, 1]++;
                aviso = "Respuesta incorrecta. Era " + respuestaCorrecta + ".";
            }

            int otro = 1 - turno;
            if (PuedeSeguir(otro))
            {
                turno = otro;
            }
            else if (!PuedeSeguir(turno))
            {
                FinalizarJuego();
                return;
            }

            GenerarPregunta();
            lblAviso.Text = aviso;
        }

        private void FinalizarJuego()
        {
            lblTituloResultados.Text = "RESULTADOS DE LA PARTIDA";
            string resumen = "";
            for (int i = 0; i < cantidadJugadores; i++)
            {
                resumen += nombres[i] + ": " + estadisticas[i, 0] + " aciertos, " +
                           estadisticas[i, 1] + " errores, " + estadisticas[i, 2] +
                           " puntos.\n";
            }

            if (cantidadJugadores == 2)
            {
                if (estadisticas[0, 2] > estadisticas[1, 2])
                    resumen += "Ganador: " + nombres[0];
                else if (estadisticas[1, 2] > estadisticas[0, 2])
                    resumen += "Ganador: " + nombres[1];
                else
                    resumen += "Resultado: empate.";
            }
            else
            {
                resumen += "Partida finalizada.";
            }

            bool historialLleno = false;
            for (int i = 0; i < cantidadJugadores; i++)
            {
                if (cantidadHistorial < 100)
                {
                    historialNombres[cantidadHistorial] = nombres[i];
                    historialPuntos[cantidadHistorial] = estadisticas[i, 2];
                    cantidadHistorial++;
                }
                else
                {
                    historialLleno = true;
                }
            }
            if (historialLleno)
                resumen += "\nEl historial esta lleno (100 registros).";

            lblResumen.Text = resumen;
            ActualizarRanking();
            MostrarPantalla(pantallaResultados);
        }

        private void ActualizarRanking()
        {
            string[] nombresOrdenados = new string[100];
            int[] puntosOrdenados = new int[100];
            for (int i = 0; i < cantidadHistorial; i++)
            {
                nombresOrdenados[i] = historialNombres[i];
                puntosOrdenados[i] = historialPuntos[i];
            }

            // Ordenamiento descendente con dos ciclos for.
            for (int i = 0; i < cantidadHistorial - 1; i++)
            {
                for (int j = i + 1; j < cantidadHistorial; j++)
                {
                    if (puntosOrdenados[j] > puntosOrdenados[i])
                    {
                        int puntosTemporal = puntosOrdenados[i];
                        puntosOrdenados[i] = puntosOrdenados[j];
                        puntosOrdenados[j] = puntosTemporal;

                        string nombreTemporal = nombresOrdenados[i];
                        nombresOrdenados[i] = nombresOrdenados[j];
                        nombresOrdenados[j] = nombreTemporal;
                    }
                }
            }

            txtRanking.Text = "";
            if (cantidadHistorial == 0)
            {
                txtRanking.Text = "Todavia no hay partidas terminadas.";
                return;
            }

            int mostrar = cantidadHistorial;
            if (mostrar > 10) mostrar = 10;
            for (int i = 0; i < mostrar; i++)
            {
                txtRanking.Text += (i + 1) + ". " + nombresOrdenados[i] +
                                   " - " + puntosOrdenados[i] + " puntos" + Environment.NewLine;
            }
        }

        private void VerRanking(object sender, EventArgs e)
        {
            lblTituloResultados.Text = "RANKING";
            lblResumen.Text = "Las puntuaciones quedan guardadas mientras el programa este abierto.";
            ActualizarRanking();
            MostrarPantalla(pantallaResultados);
        }

        private void VolverAlInicio(object sender, EventArgs e)
        {
            MostrarPantalla(pantallaInicio);
        }

        private void SalirDelJuego(object sender, EventArgs e)
        {
            DialogResult respuesta = MessageBox.Show(
                "¿Desea abandonar esta partida? No se guardara su puntuacion.",
                "Confirmar", MessageBoxButtons.YesNo);
            if (respuesta == DialogResult.Yes)
                MostrarPantalla(pantallaInicio);
        }
    }
}
