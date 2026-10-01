using System;
using System.Collections.Generic;

public class Producto
{
    public string Nombre { get; private set; }
    public decimal Precio { get; private set; }
    public int Stock { get; private set; }

    public Producto(string nombre, decimal precio, int stockInicial)
    {
        if (nombre == "" || nombre == null)
        {
            Console.WriteLine("Error: El nombre no puede estar vacio.");
            Nombre = "Sin nombre";
        }
        else
        {
            Nombre = nombre;
        }

        if (precio < 0)
        {
            Console.WriteLine("Error: El precio no puede ser negativo. Se asignara 0.");
            Precio = 0;
        }
        else
        {
            Precio = precio;
        }

        if (stockInicial < 0)
        {
            Console.WriteLine("Error: El stock no puede ser negativo. Se asignara 0.");
            Stock = 0;
        }
        else
        {
            Stock = stockInicial;
        }
    }

    public bool DescontarStock(int cantidad)
    {
        if (cantidad <= 0)
        {
            Console.WriteLine("Error: La cantidad a descontar debe ser mayor a 0.");
            return false;
        }

        if (cantidad <= Stock)
        {
            Stock = Stock - cantidad;
            return true;
        }
        else
        {
            Console.WriteLine("Error: Stock insuficiente.");
            return false;
        }
    }
}

public class Pedido
{
    private List<Producto> productos;
    public string Estado { get; private set; }

    public Pedido()
    {
        productos = new List<Producto>();
        Estado = "Pendiente";
    }

    public bool AgregarProducto(Producto p, int cant)
    {
        if (p == null)
        {
            Console.WriteLine("Error: El producto no existe.");
            return false;
        }

        bool stockDescontado = p.DescontarStock(cant);

        if (stockDescontado == true)
        {
            for (int i = 0; i < cant; i++)
            {
                productos.Add(p);
            }
            Console.WriteLine($"Se agregaron {cant} unidad(es) de {p.Nombre} al pedido.");
            return true;
        }
        else
        {
            Console.WriteLine($"No se pudo agregar {p.Nombre} al pedido por falta de stock.");
            return false;
        }
    }

    public decimal CalcularTotal(decimal impuesto)
    {
        decimal subtotal = 0;

        foreach (Producto p in productos)
        {
            subtotal = subtotal + p.Precio;
        }

        decimal total = subtotal + (subtotal * impuesto);
        return total;
    }

    public void ConfirmarPedido()
    {
        Estado = "Completado";
    }

    public List<Producto> ObtenerProductos()
    {
        return productos;
    }
}

class Program
{
    static void Main()
    {
        List<Producto> inventario = new List<Producto>()
        {
            new Producto("Notebook", 120000m, 5),
            new Producto("Mouse", 3500m, 10),
            new Producto("Teclado", 7000m, 8)
        };

        Pedido miPedido = new Pedido();
        bool salir = false;

        while (salir == false)
        {
            Console.WriteLine("\n=== MENU PRINCIPAL ===");
            Console.WriteLine("1. Ver Inventario");
            Console.WriteLine("2. Agregar Producto al Pedido");
            Console.WriteLine("3. Ver Resumen del Pedido");
            Console.WriteLine("4. Salir");
            Console.Write("Seleccione una opcion: ");

            string entradaOpcion = Console.ReadLine();
            int opcion;

            if (int.TryParse(entradaOpcion, out opcion) == true)
            {
                switch (opcion)
                {
                    case 1:
                        MostrarInventario(inventario);
                        break;

                    case 2:
                        MostrarInventario(inventario);
                        Console.Write("\nIngrese el numero de producto a comprar: ");
                        string entradaProd = Console.ReadLine();
                        int indiceProd;

                        if (int.TryParse(entradaProd, out indiceProd) == true && indiceProd > 0 && indiceProd <= inventario.Count)
                        {
                            Console.Write("Ingrese la cantidad deseada: ");
                            string entradaCant = Console.ReadLine();
                            int cantidad;

                            if (int.TryParse(entradaCant, out cantidad) == true)
                            {
                                Producto seleccionado = inventario[indiceProd - 1];
                                miPedido.AgregarProducto(seleccionado, cantidad);
                            }
                            else
                            {
                                Console.WriteLine("Error: Ingrese un numero valido para la cantidad.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Error: Seleccion de producto invalida.");
                        }
                        break;

                    case 3:
                        MostrarResumen(miPedido);
                        break;

                    case 4:
                        Console.WriteLine("¡Gracias por su compra!");
                        salir = true;
                        break;

                    default:
                        Console.WriteLine("Opcion no valida. Intente nuevamente.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Error: Por favor, ingrese un numero del menu.");
            }
        }
    }

    public static void MostrarInventario(List<Producto> lista)
    {
        Console.WriteLine("\n--- INVENTARIO DISPONIBLE ---");
        for (int i = 0; i < lista.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {lista[i].Nombre} - Precio: ${lista[i].Precio} - Stock: {lista[i].Stock}");
        }
    }

    public static void MostrarResumen(Pedido pedido)
    {
        Console.WriteLine("\n--- RESUMEN DEL PEDIDO ---");
        Console.WriteLine($"Estado del Pedido: {pedido.Estado}");

        List<Producto> prods = pedido.ObtenerProductos();

        if (prods.Count == 0)
        {
            Console.WriteLine("El pedido esta vacio.");
        }
        else
        {
            Console.WriteLine("Productos elegidos:");
            foreach (Producto p in prods)
            {
                Console.WriteLine($"- {p.Nombre}: ${p.Precio}");
            }

            decimal tasaImpuesto = 0.21m;
            decimal totalConImpuesto = pedido.CalcularTotal(tasaImpuesto);
            Console.WriteLine($"\nTotal a pagar (con IVA 21%): ${totalConImpuesto}");
        }
    }
}