using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRental.Domain.Shared;
using CarRental.Domain.VehicleModule;
using System.IO;

namespace CarRental.Domain.VehicleImageModule
{
    public class VehicleImage : BaseEntity
    {
        public int VehicleId { get; set; }
        public Bitmap? Image { get; set; }

        public VehicleImage() { }
        public VehicleImage(int id, int vehicleId, Bitmap image)
        {
            this.id = id;
            this.VehicleId = vehicleId;
            this.Image = image;
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }

        public override string Validate()
        {
            string validationResult = "VALID";
            return validationResult;
        }

        public override bool Equals(object? obj)
        {
            return obj is VehicleImage vehicleImage &&
                   EqualityComparer<int>.Default.Equals(this.VehicleId, vehicleImage.VehicleId) &&
                   EqualityComparer<Bitmap?>.Default.Equals(Image, vehicleImage.Image) &&
                   EqualityComparer<int>.Default.Equals(Id, vehicleImage.id);
        }

        public override int GetHashCode()
        {
            int hashCode = 155997214;
            hashCode = hashCode * -1521134295 + id.GetHashCode();
            hashCode = hashCode * -1521134295 + VehicleId.GetHashCode();
            hashCode = hashCode * -1521134295 + (Image != null ? Image.GetHashCode() : 0);
            return hashCode;
        }
    }
}
