using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

/*
 * Entity framework library - used to save data to a database
 *                  - is an object relational mapper ( ORM ), fancy way of saying it's a tool that allows to save
 *                    and retrive data, to and from a relational database using objects, rathen than hand coded sql queries
 *                    
 * First a new class should be added for the data that inherits "DbContext", at first DbCOntext will not be recognized
 * so "Ctrl + ." while it being selected and click "Install package EntityFramework"
 * 
 * In order for the data context to be useful, properties needs to be defined for the class, those represent
 * tables in the database. DbSet of T is an entity framework type that exposes a simple API that represents a table in the
 * database as a strongly typed collection of objects that we can use the linq query syntax to query and filter, or even
 * insert or delete records with a call to a c# method ( no sql needed to interact ).
 * The name of DbSet properties are crucial, they represent the name of the coresponding table in the database.
 * Ex. Entity Framework will query a table named Posts in the database in order to find and satisfy  
 * queries to the Post property shown below.
 * 
 * In order to be a valid database entity however, the post class must define a property that will act as the primary key
 * in the database. Entity Framework uses the convention of calling this key property ID.
 * private string _key;
 * public string key..... -> check Post as the example and other comments for further explanation  */

namespace ExploreCalifornia.Model
{
    public class BlogDataContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }

        public BlogDataContext(DbContextOptions<BlogDataContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}

/* Database further explanation for DBContext..
 * One last thing -> check if the database exist by using entity framework "Database.EnsureCreated()"
 * This checks if the database exists. If it doesn't exist, EF automatically will create the SQL schema required to create
 * the database and then go ahead to execute the generated SQL to actually create the database.  */


