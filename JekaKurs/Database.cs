using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JekaKurs
{
    class Database
    {
        const string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=FishCompany;Integrated Security=True";

        public void StartUpDatabase()
        {
            if (CheckConnection())
            {
                Debug.WriteLine("База данных уже существует и создаваться не будет!");
            }
            else
            {
                CreateDatabase();
                Debug.WriteLine("База данных успешно создана! Переходим к следующему шагу");
                Thread.Sleep(10000);
                CreateTables();
                Debug.WriteLine("Таблицы успешно созданы! Переходим к следующему шагу");
                Thread.Sleep(10000);
                InsertNativeData();
                Debug.WriteLine("Данные успешно вставлены в таблицы!");
            }
        }

        /// <summary>
        /// Проверка наличия базы данных
        /// </summary>
        /// <returns></returns>
        private bool CheckConnection()
        {
            bool checkConnection = false;

            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();

                checkConnection = true;
            }
            catch (SqlException ex)
            {
                checkConnection = false;
            }
            finally
            {
                connection.Close();
            }

            return checkConnection;
        }

        /// <summary>
        /// Создание базы данных
        /// </summary>
        private void CreateDatabase()
        {
            Directory.CreateDirectory(@"D:\FishCompany");

            String str;
            SqlConnection myConn = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Integrated security=SSPI;database=master");

            str = "CREATE DATABASE FishCompany ON PRIMARY " +
                "(NAME = FishCompany_Db, " +
                "FILENAME = 'd:\\FishCompany\\FishCompany.mdf', " +
                "SIZE = 10MB, MAXSIZE = 100MB, FILEGROWTH = 10%) " +
                "LOG ON (NAME = FishCompany_Log, " +
                "FILENAME = 'd:\\FishCompany\\FishCompanyLog.ldf', " +
                "SIZE = 5MB, " +
                "MAXSIZE = 50MB, " +
                "FILEGROWTH = 10%)" +
                "COLLATE Cyrillic_General_CI_AS";

            SqlCommand myCommand = new SqlCommand(str, myConn);

            try
            {
                myConn.Open();
                myCommand.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine("База данных не создана");
                Debug.WriteLine($"Ошибка : {ex.Message}");
            }
            finally
            {
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }
            }
        }

        /// <summary>
        /// Создание таблиц и их отношений
        /// </summary>
        private void CreateTables()
        {
            #region Sql command
            string cmd = @"CREATE TABLE Fishermens
                            (
                                Id INT IDENTITY NOT NULL PRIMARY KEY,
                                Name nvarchar(50) NOT NULL,
                                Surname nvarchar(50) NOT NULL,
                                Expirience int NOT NULL,
                                Phone nvarchar(30) NOT NULL,
                                Address nvarchar(50) NOT NULL,
                                Sex nvarchar(50) NOT NULL
                            )
                           
                            
                            CREATE TABLE Boats
                            (
                                Id int IDENTITY NOT NULL PRIMARY KEY,
                                Name nvarchar(50) NOT NULL,
                                Type nvarchar(50) NOT NULL,
                                Displacement float(3) NOT NULL,
                                DateOfConstruction date NOT NULL    
                            )

                            CREATE TABLE Fish
                            (
                                Id int IDENTITY NOT NULL PRIMARY KEY,
                                Name nvarchar(30) NOT NULL,
                                ScienceName nvarchar(50) NOT NULL,
                                Habitat nvarchar(50) NOT NULL,
                                AverageWeight float(3) NOT NULL
                            )
                            
                            CREATE TABLE Teams
                            (
                                Id int IDENTITY NOT NULL PRIMARY KEY,
                                BoatId int NOT NULL,
                                FishermenId int NOT NULL,
                                TeamId int NOT NULL,
                                Position nvarchar(50) NOT NULL,
                                ReceiptDate date NOT NULL
                            )         

                            CREATE TABLE Fishing
                            (
                                Id int IDENTITY NOT NULL PRIMARY KEY,
                                TeamId int NOT NULL,
                                FishermenId int NOT NULL,
                                FishId int NULL,
                                OutDate date NOT NULL,
                                ReturnDate date NOT NULL,
                                Weight float(3) NULL
                            )                                             
                            
                            CREATE TABLE TeamIdentification
                            (
                                Id int IDENTITY NOT NULL PRIMARY KEY,
                                Name nvarchar(40)
                            )

           
                            ALTER TABLE Teams
                            ADD CONSTRAINT FK_BoatId
                            FOREIGN KEY (BoatId) REFERENCES Boats(Id)
                            ON DELETE CASCADE

                            ALTER TABLE Teams 
                            ADD CONSTRAINT FK_FishermenId
                            FOREIGN KEY (FishermenId) REFERENCES Fishermens(Id)
                            ON DELETE CASCADE

                            ALTER TABLE Teams
                            ADD CONSTRAINT FK_TeamId
                            FOREIGN KEY (TeamId) REFERENCES TeamIdentification(Id)
                            ON DELETE CASCADE

                            ALTER TABLE Fishing
                            ADD CONSTRAINT FK_FTeamId
                            FOREIGN KEY (TeamId) REFERENCES TeamIdentification(Id)
                            ON DELETE CASCADE

                            ALTER TABLE Fishing
                            ADD CONSTRAINT FK_FishermenId
                            FOREIGN KEY (FishermenId) REFERENCES Fishermens(Id)
                            ON DELETE CASCADE

                            ALTER TABLE Fishing
                            ADD CONSTRAINT FK_FishId
                            FOREIGN KEY (FishId) REFERENCES Fish(Id)
                            ON DELETE CASCADE
";

            #endregion

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(cmd, connection);

            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                Debug.WriteLine("При создании таблиц произошла ошибка");
                Debug.WriteLine($"Ошибка : {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Заполнение таблиц данными по-умолчанию
        /// </summary>
        private void InsertNativeData()
        {

            #region Sql command

            string cmd = @"
                           INSERT INTO Fishermens
                           VALUES 
                           ('Саша',       'Иванов',     12,     '(123)-123-12-67',  'г.Киев ул Главная 10',  'Мужской'),  
                           ('Коля',       'Петров',     10,     '(124)-456-34-12',  'г.Киев ул Главная 11',  'Мужской'), 
                           ('Дима',       'Жоров',       9,     '(125)-789-12-67',  'г.Киев ул Главная 12',  'Мужской'), 
                           ('Жора',       'Клапанов',    7,     '(126)-123-34-12',  'г.Киев ул Главная 13',  'Мужской'), 
                           ('Макс',       'Флагов',      4,     '(127)-456-12-67',  'г.Киев ул Главная 14',  'Мужской'), 
                           ('Влад',       'Сольнов',     2,     '(128)-789-34-12',  'г.Киев ул Главная 15',  'Мужской'), 
                           ('Андрей',     'Наушников',   2,     '(129)-123-12-67',  'г.Киев ул Главная 16',  'Мужской'),
                           ('Виталик',    'Обоев',       2,     '(130)-456-34-12',  'г.Киев ул Главная 17',  'Мужской'), 
                           ('Тарас',      'Ленточкин',   1,     '(131)-789-12-67',  'г.Киев ул Главная 18',  'Мужской'), 
                           ('Витас',      'Декодов',     3,     '(132)-123-34-12',  'г.Киев ул Главная 19',  'Мужской');

                           INSERT INTO Boats
                           VALUES 
                           ('Быстрый',        'Надувная лодка',    100.11,      '10/11/2005'),  
                           ('Резкий',         'Надувная лодка',    100.11,      '09/12/2005'), 
                           ('Уничтожитель',   'Надувная лодка',    100.11,      '08/13/2004'), 
                           ('Рыбник',         'Надувная лодка',    100.11,      '07/14/2003'), 
                           ('Боец',           'Надувная лодка',    100.11,      '06/15/2002'), 
                           ('Ласточка',       'Надувная лодка',    100.11,      '05/16/2001'), 
                           ('Пират',          'Надувная лодка',    100.11,      '04/17/2006'),
                           ('Призрак',        'Надувная лодка',    100.11,      '03/18/2007'), 
                           ('Рыбный санитар', 'Надувная лодка',    100.11,      '02/19/2008'), 
                           ('Спец',           'Надувная лодка',    100.11,      '01/20/2009');

                           INSERT INTO Fish
                           VALUES 
                           ('Селедка',          'Herring',                       'Пресноводный',      1.65),
                           ('Окунь',            'Perca fluviatilis',             'Пресноводный',      2.65),
                           ('Толстолоб',        'Hypophthalmichthys molitrix',   'Пресноводный',      3.65),
                           ('Карась',           'Carassius gibelio',             'Пресноводный',      1.65),
                           ('Акула',            'Shark',                         'Океан',           100.65),
                           ('Рыба клоун',       'Clown Fish',                    'Океан',             2.65),
                           ('Лещ',              'Abramis brama',                 'Пресноводный',      2.65),
                           ('Белуга',           'Huso huso',                     'Анадромный',        3.65),
                           ('Жерех',            'Aspius aspius',                 'Пресноводный',      2.65),
                           ('Золотая рыбка',    'Carassius auratus',             'Пресноводный',      0.65);
                           
                           INSERT INTO TeamIdentification
                           VALUES 
                           ('Монко'),
                           ('Сапер'),
                           ('Солли'),
                           ('Долли'),
                           ('Рыбачки'),
                           ('Рыбкомнадзор'),
                           ('Дискрипция'),
                           ('Далеко'),
                           ('Белуги'),
                           ('СтопХам');

                           INSERT INTO Teams
                           VALUES 
                           ( 3,         1,         1,     'Юнга',          '10/11/2005' ),
                           ( 3,         2,         1,     'Салага',        '09/12/2005' ),
                           ( 3,         3,         1,     'Капитан',       '08/13/2004' ),
                           ( 5,         4,         2,     'Юнга',          '07/14/2003' ),
                           ( 5,         5,         2,     'Салага',        '06/15/2002' ),
                           ( 5,         6,         2,     'Капитан',       '05/16/2001' ),
                           ( 6,         7,         3,     'Юнга',          '04/17/2006' ),
                           ( 6,         8,         3,     'Салага',        '03/18/2007' ),
                           ( 6,         9,         3,     'Капитан',       '02/19/2008' ),
                           ( 6,        10,         3,     'Салага',        '01/20/2009' );
                                               
                           INSERT INTO Fishing
                           VALUES 
                           (1,     1,     1,     '10/11/2005',      '10/11/2006',        1.25),
                           (1,     1,     2,     '09/12/2005',      '09/12/2006',        1.35),
                           (2,     4,     3,     '08/13/2004',      '08/13/2005',        2.50),
                           (2,     4,     4,     '07/14/2003',      '07/14/2004',        2.70),
                           (2,     5,     5,     '06/15/2002',      '06/15/2003',      300.65),
                           (2,     6,     6,     '05/16/2001',      '05/16/2002',        1.80),
                           (2,     6,     7,     '04/17/2006',      '04/17/2007',        2.38),
                           (3,     9,     8,     '03/18/2007',      '03/18/2008',        4.11),
                           (3,     9,     9,     '02/19/2008',      '02/19/2009',        3.65),
                           (3,     9,    10,     '01/20/2009',      '01/20/2010',        1.65);                                                                    
                        ";
            #endregion

            SqlConnection connection = new SqlConnection(connectionString);

            SqlCommand command = new SqlCommand(cmd, connection);

            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                Debug.WriteLine("При создании таблиц произошла ошибка");
                Debug.WriteLine($"Ошибка : {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }

        public int DeleteTeamRelations(int id)
        {
            int deleted = 0;

            string sqlExpr = @"DELETE Teams WHERE Id = @id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(sqlExpr, connection))
                {
                    connection.Open();

                    SqlParameter parameter1 = new SqlParameter("@id", id);

                    command.Parameters.Add(parameter1);

                    deleted = command.ExecuteNonQuery();
                }
            }

            return deleted;
        }

        public int UpdateTeamRelations(string insertQuery)
        {
            int updated = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    connection.Open();

                    updated = command.ExecuteNonQuery();
                }
            }

            return updated;
        }

        public Dictionary<int,string> GetBoats()
        {
            Dictionary<int, string> boatsList = new Dictionary<int, string>();

            string sqlExpr = @"SELECT Id, Name FROM Boats";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(sqlExpr, connection))
                {
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            boatsList.Add(reader.GetInt32(0), reader.GetString(1));
                        }
                    }
                }
            }

            return boatsList;
        }

        public Dictionary<int, string> GetFishermens()
        {
            Dictionary<int, string> fishermensList = new Dictionary<int, string>();

            string sqlExpr = @"SELECT Id, Name + ' ' + Surname FROM Fishermens";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(sqlExpr, connection))
                {
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            fishermensList.Add(reader.GetInt32(0), reader.GetString(1));
                        }
                    }
                }
            }

            return fishermensList;
        }

        public Dictionary<int, string> GetFishermens(int teamId)
        {
            Dictionary<int, string> fishermensList = new Dictionary<int, string>();


            string sqlExpr = @"SELECT f.Id, f.Name + ' ' + f.Surname FROM Teams as t
                                      INNER JOIN Fishermens as f
                                      ON t.FishermenId = f.Id
                                      WHERE t.TeamId = @teamId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(sqlExpr, connection))
                {
                    connection.Open();

                    SqlParameter parameter1 = new SqlParameter("@teamId", teamId);

                    command.Parameters.Add(parameter1);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            fishermensList.Add(reader.GetInt32(0), reader.GetString(1));
                        }
                    }
                }
            }

            return fishermensList;
        }

        public Dictionary<int, string> GetFish()
        {
            Dictionary<int, string> fishList = new Dictionary<int, string>();

            string sqlExpr = @"SELECT Id, Name FROM Fish";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(sqlExpr, connection))
                {
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            fishList.Add(reader.GetInt32(0), reader.GetString(1));
                        }
                    }
                }
            }

            return fishList;
        }

        public Dictionary<int, string> GetTeams()
        {
            Dictionary<int, string> teamsList = new Dictionary<int, string>();

            string sqlExpr = @"SELECT Id, Name FROM TeamIdentification";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(sqlExpr, connection))
                {
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            teamsList.Add(reader.GetInt32(0), reader.GetString(1));
                        }
                    }
                }
            }

            return teamsList;
        }

        public int AddTeam(string addQuery)
        {
            int add = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(addQuery, connection))
                {
                    connection.Open();

                    add = command.ExecuteNonQuery();
                }
            }

            return add;
        }

        public int AddFishing(string addQuery, float weight)
        {
            int add = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(addQuery, connection))
                {
                    connection.Open();

                    SqlParameter parameter1 = new SqlParameter("@weight", weight);
                    command.Parameters.Add(parameter1);

                    add = command.ExecuteNonQuery();
                }
            }

            return add;
        }

        public int UpdateFishing(string insertQuery, float weight)
        {
            int updated = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    connection.Open();

                    SqlParameter parameter1 = new SqlParameter("@weight", weight);
                    command.Parameters.Add(parameter1);

                    updated = command.ExecuteNonQuery();
                }
            }

            return updated;
        }

        public int DeleteFishing(int id)
        {
            int deleted = 0;

            string sqlExpr = @"DELETE Fishing WHERE Id = @id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(sqlExpr, connection))
                {
                    connection.Open();

                    SqlParameter parameter1 = new SqlParameter("@id", id);

                    command.Parameters.Add(parameter1);

                    deleted = command.ExecuteNonQuery();
                }
            }

            return deleted;
        }

        public int AddBoats(string addQuery, float displac)
        {
            int add = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(addQuery, connection))
                {
                    connection.Open();

                    SqlParameter parameter1 = new SqlParameter("@displac", displac);
                    command.Parameters.Add(parameter1);

                    add = command.ExecuteNonQuery();
                }
            }

            return add;
        }

        public int UpdateBoats(string insertQuery, float displac)
        {
            int updated = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    connection.Open();

                    SqlParameter parameter1 = new SqlParameter("@displac", displac);
                    command.Parameters.Add(parameter1);

                    updated = command.ExecuteNonQuery();
                }
            }

            return updated;
        }

        public int DeleteBoat(int id)
        {
            int deleted = 0;

            string sqlExpr = @"DELETE Boats WHERE Id = @id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(sqlExpr, connection))
                {
                    connection.Open();

                    SqlParameter parameter1 = new SqlParameter("@id", id);

                    command.Parameters.Add(parameter1);

                    deleted = command.ExecuteNonQuery();
                }
            }

            return deleted;
        }

        public int AddFish(string addQuery, float weight)
        {
            int add = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(addQuery, connection))
                {
                    connection.Open();

                    SqlParameter parameter1 = new SqlParameter("@weight", weight);
                    command.Parameters.Add(parameter1);

                    add = command.ExecuteNonQuery();
                }
            }

            return add;
        }

        public int UpdateFish(string insertQuery, float weight)
        {
            int updated = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    connection.Open();

                    SqlParameter parameter1 = new SqlParameter("@weight", weight);
                    command.Parameters.Add(parameter1);

                    updated = command.ExecuteNonQuery();
                }
            }

            return updated;
        }

        public int DeleteFish(int id)
        {
            int deleted = 0;

            string sqlExpr = @"DELETE Fish WHERE Id = @id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(sqlExpr, connection))
                {
                    connection.Open();

                    SqlParameter parameter1 = new SqlParameter("@id", id);

                    command.Parameters.Add(parameter1);

                    deleted = command.ExecuteNonQuery();
                }
            }

            return deleted;
        }

        public int AddFishermen(string addQuery)
        {
            int add = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(addQuery, connection))
                {
                    connection.Open();

                    add = command.ExecuteNonQuery();
                }
            }

            return add;
        }

        public int UpdateFishermen(string insertQuery)
        {
            int updated = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    connection.Open();

                    updated = command.ExecuteNonQuery();
                }
            }

            return updated;
        }

        public int DeleteFishermen(int id)
        {
            int deleted = 0;

            string sqlExpr = @"DELETE Fishermens WHERE Id = @id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(sqlExpr, connection))
                {
                    connection.Open();

                    SqlParameter parameter1 = new SqlParameter("@id", id);

                    command.Parameters.Add(parameter1);

                    deleted = command.ExecuteNonQuery();
                }
            }

            return deleted;
        }

        public int AddTeamIdent(string addQuery)
        {
            int add = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(addQuery, connection))
                {
                    connection.Open();

                    add = command.ExecuteNonQuery();
                }
            }

            return add;
        }

        public int UpdateTeamIdent(string insertQuery)
        {
            int updated = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    connection.Open();

                    updated = command.ExecuteNonQuery();
                }
            }

            return updated;
        }

        public int DeleteTeamIdent(int id)
        {
            int deleted = 0;

            string sqlExpr = @"DELETE TeamIdentification WHERE Id = @id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(sqlExpr, connection))
                {
                    connection.Open();

                    SqlParameter parameter1 = new SqlParameter("@id", id);

                    command.Parameters.Add(parameter1);

                    deleted = command.ExecuteNonQuery();
                }
            }

            return deleted;
        }

        public int CountTeams()
        {
            int teams = 0;

            string sqlExpr = @"SELECT COUNT(Id) FROM Teams";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(sqlExpr, connection))
                {
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            teams = reader.GetInt32(0);
                        }
                    }
                }
            }

            return teams;
        }

        public int CountBoats()
        {
            int teams = 0;

            string sqlExpr = @"SELECT COUNT(Id) FROM Boats";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(sqlExpr, connection))
                {
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            teams = reader.GetInt32(0);
                        }
                    }
                }
            }

            return teams;
        }

        public int CountFishermens()
        {
            int teams = 0;

            string sqlExpr = @"SELECT COUNT(Id) FROM Fishermens";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(sqlExpr, connection))
                {
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            teams = reader.GetInt32(0);
                        }
                    }
                }
            }

            return teams;
        }

        public int CountFish()
        {
            int teams = 0;

            string sqlExpr = @"SELECT COUNT(Id) FROM Fish";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(sqlExpr, connection))
                {
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            teams = reader.GetInt32(0);
                        }
                    }
                }
            }

            return teams;
        }

        public int CountFishingTeams(int id)
        {
            int teams = 0;

            string sqlExpr = @"SELECT COUNT(Id) FROM Fishing
                                   WHERE TeamId = @id ";

            //string sqlExpr = @"SELECT COUNT(Id) FROM Fishing
            //                       GROUP BY TeamId ";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(sqlExpr, connection))
                {
                    connection.Open();

                    SqlParameter parameter1 = new SqlParameter("@id", id);

                    command.Parameters.Add(parameter1);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            teams = reader.GetInt32(0);
                        }
                    }
                }
            }

            return teams;
        }

        public int CountFishermenFish(int fisherId)
        {
            int teams = 0;

            string sqlExpr = @"SELECT COUNT(fi.Id) FROM Fishing as fi
                                INNER JOIN Fishermens as fis
                                ON fi.FishermenId = fis.Id
                                WHERE fis.Id = @fisherId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(sqlExpr, connection))
                {
                    connection.Open();

                    SqlParameter parameter1 = new SqlParameter("@fisherId", fisherId);

                    command.Parameters.Add(parameter1);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            teams = reader.GetInt32(0);
                        }
                    }
                }
            }

            return teams;
        }

        public int CountTeamsFish(int teamId)
        {
            int teams = 0;

            string sqlExpr = @"SELECT COUNT(fi.Id) FROM Fishing as fi
                                WHERE fi.Id = @teamId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(sqlExpr, connection))
                {
                    connection.Open();

                    SqlParameter parameter1 = new SqlParameter("@teamId", teamId);

                    command.Parameters.Add(parameter1);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            teams = reader.GetInt32(0);
                        }
                    }
                }
            }

            return teams;
        }

        public int GetMaxId(string tableName)
        {
            int id = -1;

            string sqlExpr = string.Format(@"SELECT Max(Id) FROM {0}",tableName);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(sqlExpr, connection))
                {
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            id = reader.GetInt32(0);
                        }
                    }
                }
            }

            return id;
        }

    }

}
