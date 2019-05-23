﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using WebsiteLaitBrasseur.BL;

namespace WebsiteLaitBrasseur.DAL
{
    [DataObject(true)]
    public class InvoiceDAL: IInvoiceDataAccess
    {
        //Get connection string from web.config file and create sql connection
        readonly SqlConnection connection = new SqlConnection(SqlDataAccess.ConnectionString);

        /// <summary>
        /// Insert a new Invoice into the DB
        /// ArrivalDate and PostageDate are calculated
        /// dependent on the shipping selected in the Business Logic Layer
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="shippingID"></param>
        /// <param name="totalQuantity"></param>
        /// <param name="totalShippingCost"></param>
        /// <param name="totalProductCost"></param>
        /// <param name="totalTaxes"></param>
        /// <param name="totalAmount"></param>
        /// <param name="orderDate"></param>
        /// <param name="paymentDate"></param>
        /// <param name="arrivalDate"></param>
        /// <param name="postageDate"></param>
        /// <param name="pamymentStatus"></param>
        /// <param name="customerMail"></param>
        /// <param name="email"></param>
        /// <returns>Invoice ID</returns>
        [DataObjectMethod(DataObjectMethodType.Insert)]
        public int Insert(int accountID, int shippingID, int totalQuantity, decimal totalShippingCost,
            decimal totalProductCost, decimal totalTaxes, decimal totalAmount, DateTime orderDate, DateTime paymentDate, 
            DateTime arrivalDate, DateTime postageDate, int paymentStatus, string customerMail)
        {
            int result;
            //no need to explicitely set id as autoincrement is used
            string queryString = "INSERT INTO dbo.Invoice(dbo.Invoice.accountID, dbo.Invoice.shippingID, dbo.Invoice.totalQuantity, dbo.Invoice.shippingCost, " +
                "dbo.Invoice.totalProductCost, dbo.Invoice.totalTax, dbo.Invoice.totalAmount, dbo.Invoice.orderDate, dbo.Invoice.paymentDate, dbo.Invoice.paymentStatus, " +
                "dbo.Invoice.customerMail, dbo.Invoice.arrivalDate, dbo.Invoice.postageDate) " +
                "VALUES(@accountID, @shippingID, @totalQuantity, @shippingCost, @totalProductCost, @totalTax, @totalAmount, @orderDate, @paymentDate, @paymentStatus " +
                "@customerMail, @arrivalDate, @postageDate)";
            string queryAutoIncr = "SELECT TOP(1) dbo.Invoice.invoiceID FROM dbo.Invoice ORDER BY 1 DESC";
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                //insert into database
                using (SqlCommand cmd = new SqlCommand(queryString, connection))
                {
                    cmd.Parameters.AddWithValue("@accountID", SqlDbType.Int).Value = accountID;
                    cmd.Parameters.AddWithValue("@shippingID", SqlDbType.Int).Value = shippingID;
                    cmd.Parameters.AddWithValue("@totalQuantity", SqlDbType.Int).Value = totalQuantity;
                    cmd.Parameters.AddWithValue("@shippingCost", SqlDbType.Decimal).Value = totalShippingCost;
                    cmd.Parameters.AddWithValue("@totalProductCost", SqlDbType.Decimal).Value = totalProductCost;
                    cmd.Parameters.AddWithValue("@totalTax", SqlDbType.Decimal).Value = totalTaxes;
                    cmd.Parameters.AddWithValue("@totalAmount", SqlDbType.Decimal).Value = totalAmount;
                    cmd.Parameters.AddWithValue("@orderDate", SqlDbType.Date).Value = orderDate;
                    cmd.Parameters.AddWithValue("@paymentDate", SqlDbType.Date).Value = paymentDate;
                    cmd.Parameters.AddWithValue("@postageDate", SqlDbType.Date).Value = postageDate;
                    cmd.Parameters.AddWithValue("@paymentStatus", SqlDbType.Bit).Value = paymentStatus;
                    cmd.Parameters.AddWithValue("@customerMail", SqlDbType.VarChar).Value = customerMail;
                    cmd.Parameters.AddWithValue("@arrivalDate", SqlDbType.Date).Value = arrivalDate;
                    cmd.CommandType = CommandType.Text;
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
                    result = (Int32)reader["invoiceID"];
                    Debug.Print("InvoiceDAL: /Insert/ " + result.ToString());
                }
                return result;
            }
            catch (Exception e)
            {
                result = 0;
                e.GetBaseException();
            }
            finally
            {
                connection.Close();
            }
            return result;
        }

        /// <summary>
        /// Update the payment status of a certain invoice
        /// in the database to paymentStatus = 1 (paied)
        ///                    paymentStatus = 0 (not-paied)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="paymentStatus"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Update)]
        public int Update(int id, int paymentStatus)
        {
            int result = 0;
            string queryString = "UPDATE dbo.Invoice SET paymentStatus = @paymentStatus WHERE invoiceID = @id";
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                //update into database where id = XY 
                using (SqlCommand cmd = new SqlCommand(queryString, connection))
                {
                    cmd.Parameters.AddWithValue("@paymentStatus", SqlDbType.Bit).Value = paymentStatus;
                    cmd.Parameters.AddWithValue("@id", SqlDbType.Int).Value = id;
                    cmd.CommandType = CommandType.Text;
                    result = cmd.ExecuteNonQuery(); //returns amount of affected rows if successfull
                    Debug.Print("InvoiceDAL: /Update/ " + result.ToString());
                }
            }
            catch (Exception e)
            {
                e.GetBaseException();
            }
            finally
            {
                connection.Close();
            }
            return result;
        }

        //update
        [DataObjectMethod(DataObjectMethodType.Update)]
        public int UpdateDate(int id, bool arrivalDate)
        {
            int result = 0;
            string queryString = "UPDATE dbo.Invoice SET arrivalDate = @arrivalDate WHERE invoiceID = @id";
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                //update into database where id = XY 
                using (SqlCommand cmd = new SqlCommand(queryString, connection))
                {
                    cmd.Parameters.AddWithValue("@arrivalDate", SqlDbType.Date).Value = arrivalDate;
                    cmd.Parameters.AddWithValue("@id", SqlDbType.Int).Value = id;
                    cmd.CommandType = CommandType.Text;
                    result = cmd.ExecuteNonQuery(); //returns amount of affected rows if successfull
                    Debug.Print("InvoiceDAL: /UpdateDate/ " + result.ToString());
                }
            }
            catch (Exception e)
            {
                e.GetBaseException();
            }
            finally
            {
                connection.Close();
            }
            return result;
        }

        /// <summary>
        /// Product selection needs to be added in Business Logic Layer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public InvoiceDTO FindBy(int id)
        {
            InvoiceDTO invoice;
            AccountDTO account;
            ShippmentDTO shipping;

            string queryString = "SELECT * FROM dbo.Invoice WHERE invoiceID = @id";

            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                //find entry in database where id = XY
                using (SqlCommand cmd = new SqlCommand(queryString, connection))
                {
                    cmd.Parameters.AddWithValue("@id", SqlDbType.Int).Value = id;
                    cmd.CommandType = CommandType.Text;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            invoice = new InvoiceDTO();
                            account = new AccountDTO();
                            shipping = new ShippmentDTO();
                            invoice = GenerateInvoice(reader, invoice, account, shipping);
                            //return product instance as data object 
                            Debug.Print("InvoiceDAL: /FindByID/ " + invoice.GetID());
                            return invoice;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                e.GetBaseException();
                Debug.Print(e.ToString());
            }
            finally
            {
                connection.Close();
            }
            return null;
        }

        /// <summary>
        /// Find all bills (invoices) that belong to a certain customer
        /// And return all as a list.
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<InvoiceDTO> FindByCustomer(int accountID)
        {
            string queryString = "SELECT * FROM dbo.Invoice WHERE accountID = @accountID";
            List<InvoiceDTO> results = new List<InvoiceDTO>();
            InvoiceDTO invoice;
            AccountDTO account;
            ShippmentDTO shipping;
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                //find entry in database where id = XY
                using (SqlCommand cmd = new SqlCommand(queryString, connection))
                {
                    cmd.Parameters.AddWithValue("@accountID", SqlDbType.Int).Value = accountID;
                    cmd.CommandType = CommandType.Text;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                invoice = new InvoiceDTO();
                                account = new AccountDTO();
                                shipping = new ShippmentDTO();
                                invoice = GenerateInvoice(reader, invoice, account, shipping);
                                //return product instance as data object 
                                Debug.Print("InvoiceDAL: /FindByCustomer/ " + invoice.GetID());

                                //add data objects to result-list 
                                results.Add(invoice);
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
            finally
            {
                connection.Close();
            }
            return null;
        }

        /// <summary>
        /// Find all invoices that are paymentStatus = 0 (unpaied) OR
        ///                            paymentStatus = 1 (paied)
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="paymentStatus"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<InvoiceDTO> FindByStatus(int accountID, int paymentStatus)
        {
            string queryString = "SELECT * FROM dbo.Invoice WHERE accountID = @accountID AND paymentStatus = @paymentStatus";
            List<InvoiceDTO> results = new List<InvoiceDTO>();
            InvoiceDTO invoice;
            AccountDTO account;
            ShippmentDTO shipping;
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                //find entry in database where id = XY
                using (SqlCommand cmd = new SqlCommand(queryString, connection))
                {
                    cmd.Parameters.AddWithValue("@accountID", SqlDbType.Int).Value = accountID;
                    cmd.Parameters.AddWithValue("@paymentStatus", SqlDbType.Bit).Value = paymentStatus;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                invoice = new InvoiceDTO();
                                account = new AccountDTO();
                                shipping = new ShippmentDTO();
                                invoice = GenerateInvoice(reader, invoice, account, shipping);
                                //return product instance as data object 
                                Debug.Print("InvoiceDAL: /FindInvoiceByStatus/ " + invoice.GetID());

                                //add data objects to result-list 
                                results.Add(invoice);
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
            finally
            {
                connection.Close();
            }
            return null;
        }

        //find all invoices per paymentdate
        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<InvoiceDTO> FindBy(DateTime paymentDate)
        {
            string queryString = "SELECT * FROM dbo.Invoice WHERE paymentDate = @paymentDate";
            List<InvoiceDTO> results = new List<InvoiceDTO>();
            InvoiceDTO invoice;
            AccountDTO account;
            ShippmentDTO shipping;
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                //find entry in database where id = XY
                using (SqlCommand cmd = new SqlCommand(queryString, connection))
                {
                    cmd.Parameters.AddWithValue("@paymentDate", paymentDate);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                invoice = new InvoiceDTO();
                                account = new AccountDTO();
                                shipping = new ShippmentDTO();
                                invoice = GenerateInvoice(reader, invoice, account, shipping);
                                //return product instance as data object 
                                Debug.Print("InvoiceDAL: /FindBy/ " + invoice.GetID());
                                //add data objects to result-list 
                                results.Add(invoice);
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
            finally
            {
                connection.Close();
            }
            return null;
        }

        /// <summary>
        /// Find all possible invoices in the database
        /// </summary>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<InvoiceDTO> FindAll()
        {
            string queryString = "SELECT * FROM dbo.Invoice";
            List<InvoiceDTO> results = new List<InvoiceDTO>();
            InvoiceDTO invoice;
            AccountDTO account;
            ShippmentDTO shipping;
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                //find entry in database where id = XY
                using (SqlCommand cmd = new SqlCommand(queryString, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                invoice = new InvoiceDTO();
                                account = new AccountDTO();
                                shipping = new ShippmentDTO();
                                invoice = GenerateInvoice(reader, invoice, account, shipping);
                                //return product instance as data object 
                                Debug.Print("InvoiceDAL: /FindAll/ " + invoice.GetID());
                                //add data objects to result-list 
                                results.Add(invoice);
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
            finally
            {
                connection.Close();
            }
            return null;
        }

        /// <summary>
        /// Find all invoices/bills which have a paymentStatus = 0 (not-paied)
        ///                                      paymentStatus = 1 (paied)
        /// </summary>
        /// <param name="paymentStatus"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<InvoiceDTO> FindAllByStatus(int paymentStatus)
        {
            string queryString = "SELECT * FROM dbo.Invoice WHERE paymentStatus = @paymentStatus";
            List<InvoiceDTO> results = new List<InvoiceDTO>();
            InvoiceDTO invoice;
            AccountDTO account;
            ShippmentDTO shipping;
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                //find entry in database where id = XY
                using (SqlCommand cmd = new SqlCommand(queryString, connection))
                {
                    cmd.Parameters.AddWithValue("@paymentStatus", paymentStatus);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                invoice = new InvoiceDTO();
                                account = new AccountDTO();
                                shipping = new ShippmentDTO();
                                invoice = GenerateInvoice(reader, invoice, account, shipping);
                                //return product instance as data object 
                                Debug.Print("InvoiceDAL: /FindAllBy/ " + invoice.GetID());

                                //add data objects to result-list 
                                results.Add(invoice);
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
            finally
            {
                connection.Close();
            }
            return null;
        }

        /// <summary>
        /// Find all invoices per customer 
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<InvoiceDTO> FindAllByCustomer(int accountID)
        {
            string queryString = "SELECT * FROM dbo.Invoice WHERE accountID = @accountID";
            List<InvoiceDTO> results = new List<InvoiceDTO>();
            InvoiceDTO invoice;
            AccountDTO account;
            ShippmentDTO shipping;
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlCommand cmd = new SqlCommand(queryString, connection))
                {
                    cmd.Parameters.AddWithValue("@accountID", accountID);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                invoice = new InvoiceDTO();
                                account = new AccountDTO();
                                shipping = new ShippmentDTO();
                                invoice = GenerateInvoice(reader, invoice, account, shipping);
                                //return product instance as data object 
                                Debug.Print("InvoiceDAL: /FindInvoiceBy/ " + invoice.GetID());

                                //add data objects to result-list 
                                results.Add(invoice);
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
            finally
            {
                connection.Close();
            }
            return null;
        }

        private static InvoiceDTO GenerateInvoice(SqlDataReader reader, InvoiceDTO invoice, AccountDTO account, ShippmentDTO shipping)
        {
            invoice.SetID(Convert.ToInt32(reader["invoiceID"]));
            account.SetID(Convert.ToInt32(reader["accountID"]));
            shipping.SetID(Convert.ToInt32(reader["shippingID"]));
            invoice.SetCustomer(account);
            invoice.SetShippment(shipping);
            invoice.SetEmail(reader["customerMail"].ToString());
            invoice.SetOrderDate(Convert.ToDateTime(reader["orderDate"]));
            invoice.SetPaymentDate(Convert.ToDateTime(reader["paymentDate"]));
            invoice.SetArrivalDate(Convert.ToDateTime(reader["arrivaltDate"]));
            invoice.SetPostDate(Convert.ToDateTime(reader["postageDate"]));
            invoice.SetQuantity(Convert.ToInt32(reader["totalQuantity"]));
            invoice.SetShippingCost(Convert.ToDecimal(reader["shippingCost"]));
            invoice.SetStatus(Convert.ToInt32(reader["paymentStatus"]));
            invoice.SetTax(Convert.ToDecimal(reader["totalTax"]));
            invoice.SetTotal(Convert.ToDecimal(reader["totalAmount"]));
            return invoice;
        }
    }
}