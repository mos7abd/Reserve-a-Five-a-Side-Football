//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Reserve__a_Five_a_Side_Football.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class Reservation
    {
        public int ReservationID { get; set; }
        public Nullable<System.DateTime> Reservation_Date { get; set; }
        public Nullable<System.TimeSpan> Reservation_Time { get; set; }
        public string Payment { get; set; }
        public string Reservation_Statues { get; set; }
        public Nullable<int> Player_ID { get; set; }
        public Nullable<int> OwnarID { get; set; }
        public Nullable<int> StadiumID { get; set; }
    
        public virtual Ownar Ownar { get; set; }
        public virtual Player Player { get; set; }
        public virtual Stadium Stadium { get; set; }
    }
}
