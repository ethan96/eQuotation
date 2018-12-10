using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.DataAccess.Entities
{
    public partial class Cart
    {
        private List<CartContact> _partners;
        public List<CartContact> Partners
        {
            get
            {
                if (_partners == null)
                {
                    _partners = new List<CartContact>();
                    if (this != null)
                    {
                        if (this.CartContact != null)
                            _partners.Add(this.CartContact);
                        if (this.CartContact1 != null)
                            _partners.Add(this.CartContact1);
                        if (this.CartContact2 != null)
                            _partners.Add(this.CartContact2);
                    }
                }
                return _partners;
            }
        }

        private CartContact _soldToContact;
        public CartContact SoldToContact
        {
            get
            {
                if (_soldToContact != null)
                    return _soldToContact;
                if (this != null && this.SoldtoID.HasValue == true)
                    _soldToContact = Partners.Where(p => p.ContactID == this.SoldtoID.Value).FirstOrDefault();
                return _soldToContact;
            }
        }

        private CartContact _shipToContact;
        public CartContact ShipToContact
        {
            get
            {
                if (_shipToContact != null)
                    return _shipToContact;
                if (this != null && this.ShiptoID.HasValue == true)
                    _shipToContact = Partners.Where(p => p.ContactID == this.ShiptoID.Value).FirstOrDefault();
                return _shipToContact;
            }
        }

        private CartContact _billToContact;
        public CartContact BillToContact
        {
            get
            {
                if (_billToContact != null)
                    return _billToContact;
                if (this != null && this.BilltoID.HasValue == true)
                    _billToContact = Partners.Where(p => p.ContactID == this.BilltoID.Value).FirstOrDefault();
                return _billToContact;
            }
        }
    }
}
