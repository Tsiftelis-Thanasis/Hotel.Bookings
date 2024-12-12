An Api with 2 controllers, for the hotels and the bookings.
Each controller contains Get, GetAll, Create and Edit method.
Each controller is using a repository for the Bookings and a repository for the Hotels.

I added a caching mechanism and a DB store mechanism.
If it's not in the cache it might be in the store, if it's not then I query the DB.

I added a small logger.
I added a background service for future use. For example if we need to update our hotels or bookings from another source or api.

I am not familiar with unit test, but I tried to do some proper testing but I didn't achieve it.
