﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebsiteLaitBrasseur.BL;

namespace WebsiteLaitBrasseur.DAL
{
    public class ProductDAL
    {
        //Get connection string from web.config file and create sql connection
        SqlConnection connection = new SqlConnection(SqlDataAccess.ConnectionString);
        //create
        public int Insert(string name, string type, string producer, string longInfo, string shortInfo, string imgPath,
            int stock, int status)
        {
            int result;
            //no need to explicitely set id as autoincrement is used
            string queryString = "INSERT INTO dbo.Product(dbo.Product.pName, dbo.Product.pType, dbo.Product.producer, dbo.Product.longInfo, dbo.Product.shortInfo, " +
                "dbo.Product.imgPath, dbo.Product.inStock, dbo.Product.pStatus) " +
                "VALUES('@name', '@type', '@producer', '@longInfo', '@shortInfo', '@imgPath', @stock, @status)";
            string queryAutoIncr = "SELECT TOP(1) dbo.Product.productID FROM dbo.Product ORDER BY 1 DESC";
            try
            {
                //insert into database
                using (SqlCommand cmd = new SqlCommand(queryString, connection))
                {
                    connection.Open();
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@password", fname);
                    cmd.Parameters.AddWithValue("@isConfirmed", isConfirmed);
                    cmd.Parameters.AddWithValue("@firstName", fname);
                    cmd.Parameters.AddWithValue("@lastName", lname);
                    cmd.Parameters.AddWithValue("@birthDate", birthdate);
                    cmd.Parameters.AddWithValue("@phone", phoneNo);
                    cmd.Parameters.AddWithValue("@imgPath", imgPath);
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.Parameters.AddWithValue("@isAdmin", isAdmin);
                    cmd.ExecuteNonQuery(); //returns amount of affected rows if successfull
                }

                ///find the last manipulated id due to autoincrement and return it
                using (SqlCommand command = new SqlCommand(queryAutoIncr, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    //won't need a while, since it will only retrieve one row
                    reader.Read();
                    //this is the id of the newly created data field
                    result = (Int32)reader["accountID"];
                    Debug.Print("AccountDAL: /Insert/ " + result.ToString());
                }
                return result;
            }
            catch (Exception e)
            {
                result = 0;
                e.GetBaseException();
            }
            return result;
        }

        public bool Create(uint id, string name, string type, string producer, float unitSize, decimal price, string info, 
            string shortInfo, string imgPath, uint stock, bool status)
        {
            try
            {
                //insert into database
                return true;
            }
            catch (Exception e)
            {
                e.GetBaseException();
            }
            return false;
        }

        //update
        //set status available or unavailable if product is not in store
        public bool Update(uint id, bool status)
        {
            try
            {
                //update into database where id = XY to status disabled(false) or enabled(true)
                return true;
            }
            catch (Exception e)
            {
                e.GetBaseException();
            }
            return false;
        }

        //update stock amount
        public bool Update(uint id, uint stock)
        {
            try
            {
                //update into database where id = XY 
                return true;
            }
            catch (Exception e)
            {
                e.GetBaseException();
            }
            return false;
        }

        //update product image
        public bool UpdateImg(uint id, string imgPath)
        {
            try
            {
                //update into database where id = XY 
                return true;
            }
            catch (Exception e)
            {
                e.GetBaseException();
            }
            return false;
        }

        //update producer
        public bool UpdateProducer(uint id, string producer)
        {
            try
            {
                //update into database where id = XY 
                return true;
            }
            catch (Exception e)
            {
                e.GetBaseException();
            }
            return false;
        }

        //update title/name
        public bool UpdateName(uint id, string name)
        {
            try
            {
                //update into database where id = XY 
                return true;
            }
            catch (Exception e)
            {
                e.GetBaseException();
            }
            return false;
        }

        //update short info
        public bool UpdateShortInfo(uint id, string longInfo)
        {
            try
            {
                //update into database where id = XY 
                return true;
            }
            catch (Exception e)
            {
                e.GetBaseException();
            }
            return false;
        }

        //update long info
        public bool UpdateLongInfo(uint id, string longInfo)
        {
            try
            {
                //update into database where id = XY 
                return true;
            }
            catch (Exception e)
            {
                e.GetBaseException();
            }
            return false;
        }

        //update complete info
        public bool Update(uint id, string longInfo, string shortInfo)
        {
            try
            {
                //update into database where id = XY 
                return true;
            }
            catch (Exception e)
            {
                e.GetBaseException();
            }
            return false;
        }

        //update unit size
        public bool Update(uint id, float unitSize)
        {
            try
            {
                //update into database where id = XY 
                return true;
            }
            catch (Exception e)
            {
                e.GetBaseException();
            }
            return false;
        }

        //update price
        public bool Update(uint id, decimal price)
        {
            try
            {
                //update into database where id = XY 
                return true;
            }
            catch (Exception e)
            {
                e.GetBaseException();
            }
            return false;
        }

        //update complete product 
        public bool Update(uint id, string name, string type, string producer, float unitSize, decimal price, string longInfo,
            string shortInfo, string imgPath, uint stock, bool status)
        {
            try
            {
                //update into database where id = XY to status disabled(false) or enabled(true)
                return true;
            }
            catch (Exception e)
            {
                e.GetBaseException();
            }
            return false;
        }

        //read
        public ProductDTO FindBy(int id)
        {
            string queryString = "SELECT * FROM dbo.Product WHERE productID=@id";
            ProductDTO product;
            try
            {                
                //find entry in database where id = XY
                using (SqlCommand cmd = new SqlCommand(queryString, connection))
                {
                    connection.Open();
                    cmd.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {                        
                        if (reader.Read())
                        {
                            product = new ProductDTO();
                            product.SetId((int)reader["productID"]);
                            product.SetName(reader["pName"].ToString());
                            product.SetType(reader["pType"].ToString());
                            product.SetProducer(reader["producer"].ToString());
                            product.SetUnit((float)reader["unitSize"]);
                            product.SetPrice((decimal)reader["unitPrice"]);
                            product.SetInfo(reader["longInfo"].ToString());
                            product.SetShortInfo(reader["shortInfo"].ToString());
                            product.SetImgPath(reader["imgPath"].ToString());
                            product.SetStock((int)reader["inStock"]);
                            product.SetStatus((int)reader["pStatus"]);
                            //return product instance as data object 
                            return product;
                        }                         
                    }
                }   
            }
            catch (Exception e)
            {
                e.GetBaseException();
            }

            return null;
        }

        //find a product by type 
        public List<ProductDTO> FindByType(string type)
        {
            List<ProductDTO> results = new List<ProductDTO>();
            string queryString = "SELECT * FROM dbo.Product WHERE pType=@type";
            try
            {
                //find entry in database where id = XY
                using (SqlCommand cmd = new SqlCommand(queryString, connection))
                {
                    connection.Open();
                    cmd.Parameters.AddWithValue("@type", type);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                ProductDTO product = new ProductDTO();
                                product.SetId((int)reader["productID"]);
                                product.SetName(reader["pName"].ToString());
                                product.SetType(reader["pType"].ToString());
                                product.SetProducer(reader["producer"].ToString());
                                product.SetUnit((float)reader["unitSize"]);
                                product.SetPrice((decimal)reader["unitPrice"]);
                                product.SetInfo(reader["longInfo"].ToString());
                                product.SetShortInfo(reader["shortInfo"].ToString());
                                product.SetImgPath(reader["imgPath"].ToString());
                                product.SetStock((int)reader["inStock"]);
                                product.SetStatus((int)reader["pStatus"]);
                                //add data objects to result-list 
                                results.Add(product);
                            }
                            return results;
                        }
                        else
                        {
                            throw new EmptyRowException();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                e.GetBaseException();
            }
            return null;
        }

        //find a product by name 
        public List<ProductDTO> FindByName(string name)
        {
            ProductDTO product;
            List<ProductDTO> list = new List<ProductDTO>();
            try
            {
                product = new ProductDTO();
                //find entry in database where id = XY
                list.Add(product);

                //after all products are retrieved from DB
                return list;
            }
            catch (Exception e)
            {
                e.GetBaseException();
            }

            return null;
        }

        //find a product by producer 
        public List<ProductDTO> FindByProducer(string producer)
        {
            ProductDTO product;
            List<ProductDTO> list = new List<ProductDTO>();
            try
            {
                product = new ProductDTO();
                //find entry in database where id = XY
                list.Add(product);

                //after all products are retrieved from DB
                return list;
            }
            catch (Exception e)
            {
                e.GetBaseException();
            }

            return null;
        }

        //find a product by price
        public List<ProductDTO> FindByPrice(decimal price)
        {
            ProductDTO product;
            List<ProductDTO> list = new List<ProductDTO>();
            try
            {
                product = new ProductDTO();
                //find entry in database where id = XY
                list.Add(product);

                //after all products are retrieved from DB
                return list;
            }
            catch (Exception e)
            {
                e.GetBaseException();
            }

            return null;
        }

        //find all products 
        public List<ProductDTO> FindAll()
        {            
            List<ProductDTO> results = new List<ProductDTO>();
            string queryString = "SELECT * FROM dbo.Product";
            try
            {
                //find entry in database where id = XY
                using (SqlCommand cmd = new SqlCommand(queryString, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                ProductDTO product = new ProductDTO();
                                product.SetId((int)reader["productID"]);
                                product.SetName(reader["pName"].ToString());
                                product.SetType(reader["pType"].ToString());
                                product.SetProducer(reader["producer"].ToString());
                                product.SetUnit((float)reader["unitSize"]);
                                product.SetPrice((decimal)reader["unitPrice"]);
                                product.SetInfo(reader["longInfo"].ToString());
                                product.SetShortInfo(reader["shortInfo"].ToString());
                                product.SetImgPath(reader["imgPath"].ToString());
                                product.SetStock((int)reader["inStock"]);
                                product.SetStatus((int)reader["pStatus"]);
                                //add data objects to result-list 
                                results.Add(product);
                            }

                            return results;
                        }
                        else
                        {
                            throw new EmptyRowException();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                e.GetBaseException();
            }

            return null;
        }
    }
}