using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.DataAccess.SAPModel
{
    public class SAPAccount
    {
        private String _soldtoid = " ";
        private String _shiptoid = " ";
        private String _companyname = " ";
        private String _countrycode = " ";
        private String _orgid = " ";
        private String _city = " ";
        private String _address = " ";
        private String _region = " ";
        private String _postalcode = " ";
        private String _tel = " ";
        private String _vatnumber = " ";
        private String _currency = " ";
        private String _taxjurisdiction = " ";
        private String _creator = " ";
        private String _createdate = DateTime.Now.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
        private String _salesgroupcode = " ";
        private String _salesofficecode = " ";
        private String _contactperson = " ";

        public String SoldtoID
        {
            get { return _soldtoid; }
            set { _soldtoid = value; }
        }
        public String ShiptoID
        {
            get { return _shiptoid; }
            set { _shiptoid = value; }
        }
        public String CompanyName
        {
            get { return _companyname; }
            set { _companyname = value; }
        }
        public String CountryCode
        {
            get { return _countrycode; }
            set { _countrycode = value; }
        }
        public String OrgID
        {
            get { return _orgid; }
            set { _orgid = value; }
        }
        public String City
        {
            get { return _city; }
            set { _city = value; }
        }
        public String Address
        {
            get { return _address; }
            set { _address = value; }
        }
        public String Region
        {
            get { return _region; }
            set { _region = value; }
        }
        public String PostalCode
        {
            get { return _postalcode; }
            set { _postalcode = value; }
        }
        public String TEL
        {
            get { return _tel; }
            set { _tel = value; }
        }
        public String VatNumber
        {
            get { return _vatnumber; }
            set { _vatnumber = value; }
        }
        public String Currency
        {
            get { return _currency; }
            set { _currency = value; }
        }
        public String TaxJurisdiction
        {
            get { return _taxjurisdiction; }
            set { _taxjurisdiction = value; }
        }
        public String Creator
        {
            get { return _creator; }
            set { _creator = value; }
        }
        public String CreateDate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        public String SalesGroupCode
        {
            get { return _salesgroupcode; }
            set { _salesgroupcode = value; }
        }
        public String SalesOfficeCode
        {
            get { return _salesofficecode; }
            set { _salesofficecode = value; }
        }
        public String ContactPerson
        {
            get { return _contactperson; }
            set { _contactperson = value; }
        }

    }

    public class SAPShipTo
    {
        private string _SHIPTOID = "";
        private string _ADDR = "";

        public string SHIPTOID
        {
            get { return _SHIPTOID; }
            set { _SHIPTOID = value; }
        }
        public string ADDR
        {
            get { return _ADDR; }
            set { _ADDR = value; }
        }
    }

}
