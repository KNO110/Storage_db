using System;
using System.Data.SqlClient;

class Program
{
    static void Main()
    {
        string connectionString = "Data Source=(local);Initial Catalog=TEST;Integrated Security=True";

        while (true)
        {
            Console.WriteLine("Выбор за тобой:");
            Console.WriteLine("1. Добавить новый товар");
            Console.WriteLine("2. Обновить информацию о существующем товаре");
            Console.WriteLine("3. Удалить товар");
            Console.WriteLine("4. Удалить поставщика");
            Console.WriteLine("5. Удалить тип товара");
            Console.WriteLine("6. Показать информацию о поставщике с наибольшим количеством товаров на складе");
            Console.WriteLine("7. Показать информацию о поставщике с наименьшим количеством товаров на складе");
            Console.WriteLine("0. Выход");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Add_product(connectionString);
                    break;
                case "2":
                    Update_product(connectionString);
                    break;
                case "3":
                    Delete_product(connectionString);
                    break;
                case "4":
                    Delete_supplier(connectionString);
                    break;
                case "5":
                    Delete_product_type(connectionString);
                    break;
                case "6":
                    Show_supplier_with_most_products(connectionString);
                    break;
                case "7":
                    Show_supplier_with_least_products(connectionString);
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Вася, ты что-то накрутил, давай по новой.");
                    break;
            }
        }
    }


    //// добавить новый товар
    static void Add_product(string connectionString)
    {
        Console.WriteLine("Введите информацию о новом товаре:");
        Console.Write("Наименование: ");
        string name = Console.ReadLine();
        Console.Write("Тип: ");
        string type = Console.ReadLine();
        Console.Write("Поставщик: ");
        string supplier = Console.ReadLine();
        Console.Write("Количество: ");
        int quantity = int.Parse(Console.ReadLine());

        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO Товары (Наименование, Тип, Поставщик, Количество) VALUES (@Name, @Type, @Supplier, @Quantity)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Type", type);
                    command.Parameters.AddWithValue("@Supplier", supplier);
                    command.Parameters.AddWithValue("@Quantity", quantity);

                    command.ExecuteNonQuery();
                    Console.WriteLine("Новый товар успешно добавлен.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    /// обновления информации о существующем товаре, а если не существует...а фиг знает, можете проверить если бд не жалко
    static void Update_product(string connectionString)
    {
        Console.WriteLine("Введите информацию о существующем товаре для обновления:");
        Console.Write("ID товара: ");
        int productId = int.Parse(Console.ReadLine());
        Console.Write("Новое количество: ");
        int quantity = int.Parse(Console.ReadLine());

        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "UPDATE Товары SET Количество = @Quantity WHERE id = @ProductId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Quantity", quantity);
                    command.Parameters.AddWithValue("@ProductId", productId);

                    command.ExecuteNonQuery();
                    Console.WriteLine("Информация о товаре успешно обновлена.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }           

    ///// удаляем какой-то товар по айди
    static void Delete_product(string connectionString)
    {
        Console.WriteLine("Введите ID товара, который хотите удалить:");
        int productId = int.Parse(Console.ReadLine());

        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "DELETE FROM Товары WHERE id = @ProductId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductId", productId);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        Console.WriteLine("Товар успешно удален.");
                    else
                        Console.WriteLine("Товар с указанным ID не найден.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    // поставщик завысил цену? ВЫРЕЖЬ ЕГО, но только из бд
    static void Delete_supplier(string connectionString)
    {
        Console.WriteLine("Введите ID поставщика, которого хотите удалить:");
        int supplierId = int.Parse(Console.ReadLine());

        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "DELETE FROM Поставщики WHERE ПоставщикИД = @SupplierId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SupplierId", supplierId);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        Console.WriteLine("Поставщик успешно удален.");
                    else
                        Console.WriteLine("Поставщик с указанным ID не найден.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    //// показывает чела у которого больше всего единиц товара на складе пылится
    static void Show_supplier_with_most_products(string connectionString)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT TOP 1 Поставщик, SUM(Количество) AS TotalQuantity " +
                               "FROM Товары " +
                               "GROUP BY Поставщик " +
                               "ORDER BY TotalQuantity DESC";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.WriteLine($"Поставщик: {reader["Поставщик"]}, Количество товаров на складе: {reader["TotalQuantity"]}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    //// У кого меньше всего товаров на складе тот показывается нам, ради премии, в виде разрыва договора:)
    static void Show_supplier_with_least_products(string connectionString)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT TOP 1 Поставщик, SUM(Количество) AS TotalQuantity " +
                               "FROM Товары " +
                               "GROUP BY Поставщик " +
                               "ORDER BY TotalQuantity";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.WriteLine($"Поставщик: {reader["Поставщик"]}, Количество товаров на складе: {reader["TotalQuantity"]}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
    

        /// не любишь какой тип товара? Ты тут бог, уничтожай
    static void Delete_product_type(string connectionString)
    {
        Console.WriteLine("Введите ID типа товара, который хотите удалить:");
        int typeId = int.Parse(Console.ReadLine());

        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "DELETE FROM ТипыТоваров WHERE id = @TypeId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TypeId", typeId);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        Console.WriteLine("Тип товара успешно удален.");
                    else
                        Console.WriteLine("Тип товара с указанным ID не найден.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }

    }
}
