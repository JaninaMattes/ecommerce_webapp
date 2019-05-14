﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebsiteLaitBrasseur.BL;

namespace WebsiteLaitBrasseur.DAL
{
    public class AccountDAL
    {
        //Get connection string from web.config file and create sql connection
        SqlConnection connection = new SqlConnection(SqlDataAccess.ConnectionString);
        //create
        public int Create(Login login, string fname, string lname, string birthdate, string phoneNo)
        {
            int result;
            //no need to explicitely set id as autoincrement is used
            //when account is created after Login, the login id needs to be set
            string queryString = "INSERT INTO Account(dbo.Account.loginId, dbo.Account.firstName, " +
                "dbo.Account.lastName, dbo.Account.birthDate, dbo.Account.phone) " +
                "VALUES(@loginId, '@firstName', '@lastName', '@birthDate', @phone)";
            try
            {
                //insert into database
                using (SqlCommand cmd = new SqlCommand(queryString, connection))
                {
                    cmd.Parameters.AddWithValue("@loginId", login);
                    cmd.Parameters.AddWithValue("@firstName", fname);
                    cmd.Parameters.AddWithValue("@lastName", lname);
                    cmd.Parameters.AddWithValue("@birthDate", birthdate);
                    cmd.Parameters.AddWithValue("@phone", phoneNo);
                    connection.Open();
                    result = cmd.ExecuteNonQuery(); //returns 1 if successfull
                    return result;
                }
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

        public bool Create(byte id, string fname, string lname, string birthdate, string phoneNo, bool status, bool isAdmin)
        {
            try
            {
                //insert into database 
                //status = true(not suspendet)/false(suspendet)
                //isAdmin = true / false
                return true;
            }
            catch (Exception e)
            {
                e.GetBaseException();
            }
            return false;
        }

        public bool Create(byte id, string fname, string lname, string birthdate, string phoneNo, bool status, bool isAdmin, Address address)
        {
            try
            {
                //insert into database 
                //status = true(not suspendet)/false(suspendet)
                //isAdmin = true / false
                return true;
            }
            catch (Exception e)
            {
                e.GetBaseException();
            }
            return false;
        }

        //update
        public bool Update(byte id, bool status)
        {
            try
            {
                //update into database where id = XY to status suspendet(false) or enabled(true) 
                //e.g. after three false log in attempts / upaied bills
                return true;
            }
            catch (Exception e)
            {
                e.GetBaseException();
            }
            return false;
        }

        public bool Update(byte id, string phoneNo)
        {
            try
            {
                //update into database where id = XY to status suspendet(false) or enabled(true) 
                //e.g. after three false log in attempts / upaied bills
                return true;
            }
            catch (Exception e)
            {
                e.GetBaseException();
            }
            return false;
        }

        public bool Update(byte id, Address address)
        {
            try
            {
                //update into database where id = XY to status suspendet(false) or enabled(true) 
                //e.g. after three false log in attempts / upaied bills
                return true;
            }
            catch (Exception e)
            {
                e.GetBaseException();
            }
            return false;
        }

        public bool Update(byte id, string imgPath)
        {
            try
            {
                //update into database where id = XY and set new image
                //e.g. after three false log in attempts / upaied bills
                return true;
            }
            catch (Exception e)
            {
                e.GetBaseException();
            }
            return false;
        }

        public bool Update(byte id, string fname, string lname, string birthdate, string phoneNo, bool status, bool isAdmin, Address address)
        {
            try
            {
                //insert into database 
                //status = true(not suspendet)/false(suspendet)
                //isAdmin = true / false
                return true;
            }
            catch (Exception e)
            {
                e.GetBaseException();
            }
            return false;
        }

        //read
        //find one specific person in the database
        public Account FindBy(byte id)
        {
            Account account;
            try
            {
                account = new Account();
                //find entry in database where id = XY
                return account;
            }
            catch (Exception e)
            {
                e.GetBaseException();
            }

            return null;
        }
        //find all admins in the database
        public Account FindBy(bool isAdmin)
        {
            Account account;
            try
            {
                account = new Account();
                //find entry in database where isAdmin = true (Admin) else Customer
                return account;
            }
            catch (Exception e)
            {
                e.GetBaseException();
            }

            return null;
        }

        public Account FindByStatus(bool status)
        {
            Account account;
            try
            {
                account = new Account();
                //find entry in database where status = suspendet/enabled
                return account;
            }
            catch (Exception e)
            {
                e.GetBaseException();
            }

            return null;
        }

        //find person in database by name
        public Account FindBy(string fname, string lname)
        {
            Account account;
            try
            {
                account = new Account();
                //find entry in database where name = fname + lname
                return account;
            }
            catch (Exception e)
            {
                e.GetBaseException();
            }

            return null;
        }

    }
}