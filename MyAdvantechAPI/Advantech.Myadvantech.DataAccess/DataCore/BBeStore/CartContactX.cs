using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.DataAccess.Entities
{
    public partial class CartContact
    {
        /// <summary>
        /// Add address validation status for ship to address
        /// </summary>
        public enum AddressValidationStatus
        {
            Unknown,
            Invalid,
            POBOX,
            Valid,
            CustomerConfirmed,
            CCRConfirmed
        }

        /// <summary>
        /// Add address validationX status for ship to address
        /// </summary>
        public AddressValidationStatus ValidationStatusX
        {
            get
            {
                AddressValidationStatus status = AddressValidationStatus.Unknown;
                if (!string.IsNullOrEmpty(this.ValidationStatus))
                    Enum.TryParse<AddressValidationStatus>(this.ValidationStatus, out status);
                return status;
            }
        }
        /// <summary>
        /// Check ship to address
        /// </summary>
        public bool ToBeVerifiedShipToAddress
        {
            get
            {
                bool result = false;
                switch (this.ValidationStatusX)
                {
                    case AddressValidationStatus.Invalid:
                    case AddressValidationStatus.POBOX:
                    case AddressValidationStatus.CustomerConfirmed:
                        result = true;
                        break;
                    case AddressValidationStatus.CCRConfirmed:
                    case AddressValidationStatus.Unknown:
                    case AddressValidationStatus.Valid:
                    default:
                        result = false;
                        break;
                }
                return result;
            }
        }
    }
}
