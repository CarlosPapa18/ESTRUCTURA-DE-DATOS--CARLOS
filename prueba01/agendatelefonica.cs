// AGENDA TELEFÓNICA EN C# CON PROGRAMACIÓN ORIENTADA A OBJETOS (POO)
using System;
using System.Collections.Generic;

namespace AgendaTelefonicaApp
{
    class Contacto
    {
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }

        // Constructor para inicializar el contacto con datos
        public Contacto(string nombre, string telefono, string email)
        {
            Nombre = nombre;
            Telefono = telefono;
            Email = email;
        }
        public void Mostrar()
        {
            Console.WriteLine($"Nombre   : {Nombre}");
            Console.WriteLine($"Teléfono : {Telefono}");
            Console.WriteLine($"Email    : {Email}");
            Console.WriteLine(new string('-', 30));
        }
    }

    //  MOLDE PARA LA AGENDA (Maneja la colección de contactos)
    class Agenda
    {
        private List<Contacto> listaContactos;

        public Agenda()
        {
            listaContactos = new List<Contacto>();
        }

        // Acción: Registrar un nuevo contacto
        public void AgregarContacto(Contacto nuevoContacto)
        {
            listaContactos.Add(nuevoContacto);
            Console.WriteLine("\n¡Contacto guardado con éxito!");
        }

        // Acción: Mostrar todos los elementos guardados
        public void MostrarTodos()
        {
            if (listaContactos.Count == 0)
            {
                Console.WriteLine("\nLa agenda está vacía.");
                return;
            }

            Console.WriteLine($"\n=== LISTA DE CONTACTOS ({listaContactos.Count}) ===");
            foreach (var contacto in listaContactos)
            {
                contacto.Mostrar();
            }
        }
    }

    // 3. EL MOTOR DEL PROGRAMA (Interacción con el usuario)
    class Program
    {
        static void Main(string[] args)
        {
            Agenda miAgenda = new Agenda();
            bool ejecutanado = true;

            while (ejecutanado)
            {
                Console.Clear();
                Console.WriteLine("=====================================");
                Console.WriteLine("       AGENDA TELEFÓNICA POO         ");
                Console.WriteLine("=====================================");
                Console.WriteLine("1. Ver todos los contactos");
                Console.WriteLine("2. Agregar un nuevo contacto");
                Console.WriteLine("3. Salir");
                Console.WriteLine("=====================================");
                Console.Write("Selecciona una opción (1-3): ");

                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        Console.Clear();
                        miAgenda.MostrarTodos();
                        PresioneContinuar();
                        break;

                    case "2":
                        Console.Clear();
                        Console.WriteLine("=== REGISTRAR NUEVO CONTACTO ===");
                        
                        // Capturamos los datos desde el teclado con variables string
                        Console.Write("Ingrese el Nombre: ");
                        string nombre = Console.ReadLine();

                        Console.Write("Ingrese el Teléfono: ");
                        string telefono = Console.ReadLine();

                        Console.Write("Ingrese el Email: ");
                        string email = Console.ReadLine();

                        // Creamos el objeto Contacto y lo enviamos a la agenda
                        Contacto nuevo = new Contacto(nombre, telefono, email);
                        miAgenda.AgregarContacto(nuevo);
                        
                        PresioneContinuar();
                        break;

                    case "3":
                        ejecutanado = false;
                        Console.WriteLine("\n¡Gracias por usar la agenda! Saliendo del sistema...");
                        break;

                    default:
                        Console.WriteLine("\nOpción inválida. Intente de nuevo.");
                        PresioneContinuar();
                        break;
                }
            }
        }

        static void PresioneContinuar()
        {
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }
}