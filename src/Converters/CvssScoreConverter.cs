using nuget_audit.Enums;

namespace nuget_audit.Converters
{
    public static class CvssScoreConverter
    {
        public static Severity ConvertScoreToSeverity(double score)
        {
            if (score < 0.1)
            {
                return Severity.None;
            }
            else if(score >= 0.1 && score < 3.9)
            {
                return Severity.Low;
            }
            else if (score >= 3.9 && score < 6.9)
            {
                return Severity.Medium;
            }
            else if (score >= 6.9 && score < 8.9)
            {
                return Severity.High;
            }
            else
            {
                return Severity.Crticial;
            }
        }
    }
}