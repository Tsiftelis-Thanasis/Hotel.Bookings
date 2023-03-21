alter table dbo.Bookings
add CONSTRAINT FK_HotelId FOREIGN KEY (HotelId)
    REFERENCES Hotels(Id)