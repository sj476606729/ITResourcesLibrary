using Bmob_space;
using ITResourceLibrary.Business.Models;
using ITResourceLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITResourceLibrary.Business
{
    public class AccountOperation
    {
        public static List<AccountModel> AllAccount;
        BaseOperation<AccountModel, BmobUserModel> User = new BaseOperation<AccountModel, BmobUserModel>();
        public string AllowAccount(AccountViewModel account)
        {
            if (AllAccount == null)
            {
                AllAccount = User.GetAll("_User").ToList();
            }
            var data = from a in AllAccount where a.username == account.User && a.Password == account.Password select a;
            if (data.Count() == 1)
            {
                return data.Single().Name;
            }
            else {
                var result = User.GetByNoId("_User", "username", account.User);
                if (result.Count() == 1)
                {
                    if (result.Single().Password == account.Password)
                    {
                        AllAccount.Add(result.Single());
                        return result.Single().Name;
                    }
                    else return null;
                }
                else return null;
                
            }
        }
    }
}