using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;//ADO.NET classes

namespace ADO.NET
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello ADO");
			const int PADDING = 33;
			const string CONNECTION_STRING =
				"Data Source=(localdb)\\MSSQLLocalDB;" +
				"Initial Catalog=Movies_VPD_311;" +
				"Integrated Security=True;" +
				"Connect Timeout=30;" +
				"Encrypt=False;" +
				"TrustServerCertificate=False;" +
				"ApplicationIntent=ReadWrite;" +
				"MultiSubnetFailover=False";
			Console.WriteLine(CONNECTION_STRING);

			//1)Создаем подключение к базе
			SqlConnection connection = new SqlConnection(CONNECTION_STRING);

			//2)Создаем команду, которую хотим выполнить на сервере
			string cmd = "SELECT * FROM Movies";
			SqlCommand command = new SqlCommand(cmd, connection);

			//3) Получаем результаты запроса с сервера:
			connection.Open();
			SqlDataReader reader = command.ExecuteReader();

			//4)Обрабатываем результаты запроса:
			if (reader.HasRows)
			{
				for (int i = 0; i < reader.FieldCount; i++)
					Console.Write(reader.GetName(i).ToString().PadRight(PADDING));
						Console.WriteLine();
				while (reader.Read())
				{
					//Console.WriteLine($"{reader[0]}\t{reader[1]}\t{reader[2]}\t");
					for (int i = 0; i < reader.FieldCount; i++)
					{
						Console.Write(reader[i].ToString().PadRight(PADDING));
					}
					Console.WriteLine();
				}
			}
			//закрываем поток и соединение с сервером
			reader.Close();
			connection.Close();
		}
	}
}
/*
ADO.NET - ActiveX Data Objects - это технология для работы с базами данных на С# коде. ADO позволяет любому .NET
приложению использовать базу данных. ADO.NET реализован в виде классов из пространства имен System.Data.SqlClient.
Включает в себя такие классы как 
	- SqlConnection - описывает сетевое соединение с базой данных;
	- SqlCommand    - описывает команду, отправляемую на сервер;
	- SqlDataReader - описывает поток данных от сервера к клиентскому приложению,
					  а так же позволяет отправлять команды на сервер;
	
	ADO так же позволяет хранить какую-то часть базы данных на локальной машине, для этого используется
DataSet - обеспечивает локальное хранилище данных из базы, для того чтобы получить эти данные из базы используется
DataAdapter - является посредником между DataSet и источником данных
DataAdapter - также включает в себя  SqlCommand и SqlDataReader и неявно использует их для работы с базой
*/