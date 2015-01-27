using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
/// <summary>
/// Summary description for Demon Ha
/// </summary>

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class Demo : System.Web.Services.WebService
{

    [WebMethod(EnableSession = true)]
    public string TestDemo()
    {

        return "called";
    }

    [WebMethod(EnableSession = true)]
    public List<Country> loadCountries()
    {
        List<Country> loCountriesList = new List<Country>();
        using (HRMSysLinQDataContext loDataContext = new HRMSysLinQDataContext())
        {
            loCountriesList = (from c in loDataContext.Country_Masts
                               orderby c.CountryName
                               select new Country
                               {
                                   //inCountryId = c.Id,
                                   strCountryId = c.Id.ToString(),
                                   stCountry = c.CountryName
                               }).ToList();
        }

        //loCountriesList.Insert(0, new Country { inCountryId = 0, stCountry = "--Select--" });
        loCountriesList.Insert(0, new Country { strCountryId = "", stCountry = "--Select--" });

        return loCountriesList;
    }

    [WebMethod(EnableSession = true)]
    public List<State> loadStates(int IntCountryId)
    {
        List<State> loadStatesList = new List<State>();

        using (HRMSysLinQDataContext loadDataContext = new HRMSysLinQDataContext())
        {
            loadStatesList = (from s in loadDataContext.State_Masts
                              where s.CountryId == IntCountryId
                              orderby s.StateName
                              select new State
                              {
                                  //intStateId = s.Id,
                                  strStateId = s.Id.ToString(),
                                  strState = s.StateName
                              }).ToList();
        }
        State loState = new State();
        //loState.intStateId = 0;
        loState.strStateId = "";
        loState.strState = "--Select--";
        loadStatesList.Insert(0, loState);
        return loadStatesList;
    }

    [WebMethod(EnableSession = true)]
    public List<City> loadCities(int IntCountryId, int IntStateId)
    {
        List<City> loadcitylist = new List<City>();

        using (HRMSysLinQDataContext loadDataContext = new HRMSysLinQDataContext())
        {
            loadcitylist = (from c in loadDataContext.City_Masts
                            where c.CountryId == IntCountryId && c.StateId == IntStateId
                            orderby c.CityName
                            select new City
                            {
                                //intCityId = c.Id,
                                strCityId = c.Id.ToString(),
                                strCity = c.CityName
                            }).ToList();
        }

        //loadcitylist.Insert(0, new City { intCityId = 0, strCity = "--Select--" });
        loadcitylist.Insert(0, new City { strCityId = "", strCity = "--Select--" });

        return loadcitylist;
    }

    public class Country
    {
        //public int inCountryId;
        public string strCountryId;
        public string stCountry;
    }

    public class State
    {
        //public int intStateId;
        public string strStateId;
        public string strState;
    }

    public class City
    {
        //public int intCityId;
        public string strCityId;
        public string strCity;
    }
}
