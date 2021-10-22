using System;

namespace Library.Data.Gps
{
    public static class Functions
    {
        #region [Local methods for calculations]

        /// <summary>
        /// Normalises/returns Modular of double degrees passed in
        /// </summary>
        /// <param name="degrees"></param>
        /// <returns></returns>
        private static double Normalise(double degrees)
        {
            double degs = degrees;

            while (degs < 0)
                degs += 360;

            degs = degs % 360;

            return degs;
        }

        /// <summary>
        /// Converts Radians to Degrees
        /// </summary>
        /// <param name="radians"></param>
        /// <returns></returns>
        private static double RadToDeg(double radians)
        {
            return radians * (180 / Math.PI);
        }

        /// <summary>
        /// Converts Degrees to Radians
        /// </summary>
        /// <param name="degrees"></param>
        /// <returns></returns>
        private static double DegToRad(double degrees)
        {
            return degrees * (Math.PI / 180);
        }

        /// <summary>
        /// Splits NMEA time and seperates it with : for UK time format
        /// </summary>
        /// <param name="nmeaTime"></param>
        /// <returns></returns>
        private static string SplitStringToTimeFormat(string nmeaTime)
        {
            return nmeaTime.Substring(0, 2) + ":" +
                   nmeaTime.Substring(2, 2) + ":" +
                   nmeaTime.Substring(4, 2);
        }

        /// <summary>
        /// Splits NMEA string and seperates it with / for UK date formatting
        /// </summary>
        /// <param name="nmeaDate"></param>
        /// <returns></returns>
        private static string SplitStringToDateFormat(string nmeaDate)
        {
            return nmeaDate.Substring(0, 2) + "/" +
                   nmeaDate.Substring(2, 2) + "/20" +
                   nmeaDate.Substring(4, 2);
        }

        #endregion

        public class Waypoint
        {
            public string WaypointId { get; set; }
            public double Altitude { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }

        public static class Constants
        {
            /// <summary>
            /// Returns the Earths radius in Kilometers
            /// </summary>
            public static readonly double EarthRadius = 6371; //Km

            /// <summary>
            /// Returns the Earths diameter in Kilometers
            /// </summary>
            public static readonly double EarthDiameter = 12742; //Km
        }

        public static class NmeaSentence
        {
            /// <summary>
            /// Returns a checksum for the NMEA sentence passed in
            /// </summary>
            /// <param name="sentence"></param>
            /// <returns></returns>
            public static string GenerateChecksum(string sentence)
            {
                int checksum = 0;

                foreach (char character in sentence)
                {
                    switch (character)
                    {
                        case '$':
                            break;
                        case '*':
                            continue;
                        default:
                            if (checksum == 0)
                                checksum = Convert.ToByte(character);
                            else
                                checksum = checksum ^ Convert.ToByte(character);
                            break;
                    }
                }
                return checksum.ToString("X2");
            }

            /// <summary>
            /// Extracts GPRMC Sentence only from the given GPS data
            /// </summary>
            /// <param name="gpsData"></param>
            /// <returns></returns>
            public static string ExtractGprmcSentence(string gpsData)
            {
                string[] sentences = gpsData.Split('$');
                for (int cnt = 0; cnt < sentences.Length; cnt++)
                {
                    if (sentences[cnt].StartsWith("GPRMC"))
                    {
                        return "$GP" + sentences[cnt];
                    }
                }
                return "[NO_GPRMC_SENTENCE_PRESENT]";
            }

            /// <summary>
            /// Extracts GPGGA Sentence only from the given GPS data
            /// </summary>
            /// <param name="gpsData"></param>
            /// <returns></returns>
            public static string ExtractGpggaSentence(string gpsData)
            {
                string[] sentences = gpsData.Split('$');
                for (int cnt = 0; cnt < sentences.Length; cnt++)
                {
                    if (sentences[cnt].StartsWith("GPGGA"))
                    {
                        return "$GP" + sentences[cnt];
                    }
                }
                return "[NO_GPGGA_SENTENCE_PRESENT]";
            }

            /// <summary>
            /// Extracts GPGSA Sentence only from the given GPS data
            /// </summary>
            /// <param name="gpsData"></param>
            /// <returns></returns>
            public static string ExtractGpgsaSentence(string gpsData)
            {
                string[] sentences = gpsData.Split('$');
                for (int cnt = 0; cnt < sentences.Length; cnt++)
                {
                    if (sentences[cnt].StartsWith("GPGSA"))
                    {
                        return "$GP" + sentences[cnt];
                    }
                }
                return "[NO_GPGSA_SENTENCE_PRESENT]";
            }
        }

        public static class Coordinates
        {
            /// <summary>
            /// Converts Decimal coordinate into NMEA format
            /// </summary>
            /// <param name="position"></param>
            /// <returns></returns>
            public static double DecimalToNmea(double position)
            {
                double result = 0;
                int degrees = (int)position;
                double minutes = (position - degrees) * 60;
                result = (degrees * 100) + minutes;

                if (result < 0)
                    result = result * -1;

                return result;
            }

            /// <summary>
            /// Converts NMEA coordinate into Decimal format
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public static double NmeaToDecimal(double value)
            {
                int degrees = (int)value / 100;
                double minutes = value - degrees * 100;
                return degrees + minutes / 60;
            }
        }

        public static class Speed
        {
            public static double MphToKnots(double mph)
            {
                return mph / 1.15077945;
            }

            public static double KphToKnots(double kph)
            {
                return kph / 1.85200;
            }

            public static double KnotsToMph(double knots)
            {
                return knots * 1.15077945;
            }

            public static double KnotsToKph(double knots)
            {
                return knots * 1.85200;
            }
        }

        public static class Distance
        {
            /// <summary>
            /// Converts Nautical Miles to Statute Miles
            /// </summary>
            /// <param name="nauticalMile"></param>
            /// <returns></returns>
            public static double NauticalMilesToStatuteMiles(double nauticalMile)
            {
                return nauticalMile * 1.15077945;
            }

            /// <summary>
            /// Converts Nautical Miles to Kilometers
            /// </summary>
            /// <param name="nauticalMiles"></param>
            /// <returns></returns>
            public static double NauticalMilesToKm(double nauticalMiles)
            {
                return nauticalMiles * 1.85200;
            }

            public static double KmToNauticalMiles(double kilometers)
            {
                return kilometers / 1.85200;
            }

            public static double MetersToFeet(double meters)
            {
                return meters * 3.2808399;
            }

            public static double FeetToMeters(double feet)
            {
                return feet / 3.2808399;
            }

            /// <summary>
            /// Calculates the distance between two GPS points on the globe
            /// </summary>
            /// <param name="fromLat"></param>
            /// <param name="fromLon"></param>
            /// <param name="toLat"></param>
            /// <param name="toLon"></param>
            /// <returns></returns>
            public static double DistanceBetweenPoints(double fromLat, double fromLon, double toLat, double toLon)
            {
                double earthRadius = Constants.EarthRadius;

                double dLat = DegToRad(toLat - fromLat);
                double dLon = DegToRad(fromLon - toLon);
                double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                           Math.Cos(DegToRad(fromLat)) * Math.Cos(DegToRad(toLat)) *
                           Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
                double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

                double kilometers = earthRadius * c;
                double nauticalMiles = (kilometers / 1.609344) * 0.8684;

                return nauticalMiles;
            }

        }

        public static class DateTimeFormats
        {
            /// <summary>
            /// Returns a string, formatted to NMEA standard for sentence generation (xxxx.xxxx)
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public static string NmeaForGpsOutput(double value)
            {
                return String.Format("{0:0000.0000}", value);
            }

            /// <summary>
            /// Converts NMEA formatted time to UK format time string
            /// </summary>
            /// <param name="nmeaTime"></param>
            /// <returns></returns>
            public static string NmeaTimeToString(string nmeaTime)
            {
                return SplitStringToTimeFormat(nmeaTime);
            }

            /// <summary>
            /// Converts NMEA formatted date to UK format date string
            /// </summary>
            /// <param name="nmeaDate"></param>
            /// <returns></returns>
            public static string NmeaDateToString(string nmeaDate)
            {
                return SplitStringToDateFormat(nmeaDate);
            }

            /// <summary>
            /// Returns date/time from NMEA Time
            /// </summary>
            /// <param name="nmeaTime"></param>
            /// <returns></returns>
            public static DateTime NmeaTimeToDate(string nmeaTime)
            {
                string newTime = SplitStringToTimeFormat(nmeaTime);
                return Convert.ToDateTime(newTime);
            }
        }

        public static class Navigation
        {
            public static string CardinalDirFromBearing(int bearing)
            {
                string compassDir = string.Empty;

                if ((bearing == 0) || (bearing == 360)) compassDir = "N";
                else if ((bearing >= 1) && (bearing <= 44)) compassDir = "NNE";
                else if (bearing == 45) compassDir = "NE";
                else if ((bearing >= 46) && (bearing <= 89)) compassDir = "ENE";

                if (bearing == 90) compassDir = "E";
                else if ((bearing >= 91) && (bearing <= 134)) compassDir = "ESE";
                else if (bearing == 135) compassDir = "SE";
                else if ((bearing >= 136) && (bearing <= 179)) compassDir = "SSE";

                if (bearing == 180) compassDir = "S";
                else if ((bearing >= 181) && (bearing <= 224)) compassDir = "SSW";
                else if (bearing == 225) compassDir = "SW";
                else if ((bearing >= 226) && (bearing <= 269)) compassDir = "WSW";

                if (bearing == 270) compassDir = "W";
                else if ((bearing >= 271) && (bearing <= 314)) compassDir = "WNW";
                else if (bearing == 315) compassDir = "NW";
                else if ((bearing >= 316) && (bearing <= 359)) compassDir = "NNW";

                return compassDir;
            }

            /// <summary>
            /// Calculates the bearing between two GPS coordinates
            /// </summary>
            /// <param name="latA"></param>
            /// <param name="lonA"></param>
            /// <param name="latB"></param>
            /// <param name="lonB"></param>
            /// <returns></returns>
            public static int CalculateBearing(double latA, double lonA, double latB, double lonB)
            {
                double lat1 = DegToRad(latA);
                double lat2 = DegToRad(latB);

                double dLon = DegToRad(lonB - lonA);

                double y = Math.Sin(dLon) * Math.Cos(lat2);
                double x = Math.Cos(lat1) * Math.Sin(lat2) -
                           Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(dLon);

                double retVal = RadToDeg(Math.Atan2(y, x));
                retVal = Normalise(retVal);

                return (int)retVal;
                //return (this.toDeg() + 360) % 360;
            }

            /// <summary>
            /// Returns (L)eft or (R)ight turn from current heading and required bearing
            /// </summary>
            /// <param name="heading"></param>
            /// <param name="bearing"></param>
            /// <returns></returns>
            public static char CalculateTurn(int heading, int bearing)
            {
                char leftRight = '\0';

                if (heading < bearing)
                    leftRight = 'L';
                else if (heading > bearing)
                    leftRight = 'R';

                return leftRight;
            }

            /// <summary>
            /// Returns Cardinal direction for Longitude as (E)ast or (W)est
            /// </summary>
            /// <param name="longitude"></param>
            /// <returns></returns>
            public static char CalculateLonCardinalDir(double longitude)
            {
                if (longitude > 0)
                    return 'E';

                return 'W';
            }

            /// <summary>
            /// Returns Cardinal direction for Latitude as (N)orth or (S)outh
            /// </summary>
            /// <param name="latitude"></param>
            /// <returns></returns>
            public static char CalculateLatCardinalDir(double latitude)
            {
                if (latitude > 0)
                    return 'N';

                return 'S';
            }

            /// <summary>
            /// Calculates the cross track error (xTE), giving the distance away (in NM) to the line of direction to a given Waypoint
            /// </summary>
            /// <param name="waypoint1"></param>
            /// <param name="waypoint2"></param>
            /// <param name="currentLat"></param>
            /// <param name="currentLon"></param>
            /// <returns></returns>
            public static double CalculateCrossTrackError(Waypoint waypoint1, Waypoint waypoint2, double currentLat,
                double currentLon)
            {
                int crossTrackAngle;

                double currentDistanceFromLastWaypoint = Distance.DistanceBetweenPoints(waypoint1.Latitude,
                    waypoint1.Longitude, currentLat, currentLon);
                int bearingFromLastToNextWaypoint = CalculateBearing(waypoint1.Latitude, waypoint1.Longitude,
                    waypoint2.Latitude, waypoint2.Longitude);
                int currentBearingFromLastWaypoint = CalculateBearing(waypoint1.Latitude, waypoint1.Longitude,
                    currentLat, currentLon);

                if (bearingFromLastToNextWaypoint > currentBearingFromLastWaypoint)
                    crossTrackAngle = bearingFromLastToNextWaypoint - currentBearingFromLastWaypoint;
                else
                    crossTrackAngle = currentBearingFromLastWaypoint - bearingFromLastToNextWaypoint;

                double crossTrackError = Math.Sin(crossTrackAngle) * currentDistanceFromLastWaypoint;

                if (crossTrackError < 0)
                    crossTrackError = crossTrackError * -1;

                return crossTrackError; // returned as NM 
            }
        }
    }
}