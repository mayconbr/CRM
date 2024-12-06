namespace CRMAudax.Models
{
    public class AuxQuod
    {
        public class PersonName
        {
            public Date DateLastSeen { get; set; }
            public Name Name { get; set; }
        }

        public class MobilePhoneNumber
        {
            public string PhoneNumber { get; set; }            
        }

        public class Name
        {
            public string Full { get; set; }
        }

        public class MotherName
        {
            public string Full { get; set; }
        }

        public class DOB
        {
            public string Year { get; set; }
            public string Month { get; set; }
            public string Day { get; set; }
        }

        public class Date
        {
            public string Year { get; set; }
            public string Month { get; set; }
            public string Day { get; set; }
        }

        public class Address
        {
            public string Street { get; set; }
            public string Number { get; set; }
            public string Neighborhood { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string PostalCode { get; set; }
            public Date DateLastSeen { get; set; }
        }

        public class Email
        {
            public Date DateLastSeen { get; set; }
            public string email { get; set; }
        }

        public class PhoneNumber
        {
            public Date DateLastSeen { get; set; }
            public string Number { get; set; }
        }

        public class Person
        {
            public string CPStatus { get; set; }
            public string Segment { get; set; }
            public bool HasOnlyMinimumPII { get; set; }
            public bool HasNegativeData { get; set; }
            public bool HasInquiryData { get; set; }
            public BestInfo BestInfo { get; set; }         
            public QuodScore QuodScore { get; set; }

            public Negative Negative { get; set; }

            public Protests Protests { get; set; }
        }

        public class BestInfo
        {
            public string CPF { get; set; }
            public string CPFStatus { get; set; }
            public PersonName PersonName { get; set; }
            public MotherName MotherName { get; set; }
            public DOB DOB { get; set; }
            public string Age { get; set; }
            public string Gender { get; set; }
            public Address Address { get; set; }
            public Email Email { get; set; }
            public PhoneNumber PhoneNumber { get; set; }
            public MobilePhoneNumber MobilePhoneNumber { get; set; }
        }

        public class QuodReportResponseEx
        {
            public Response Response { get; set; }
            public QuodScore QuodScore { get; set; }
            public Negative Negative { get; set; }
            public CcfApontamentos CcfApontamentos { get; set; }
            public Inquiries Inquiries { get; set; }
            public Protests Protests { get; set; }
        }

        public class Header
        {
            public int Status { get; set; }
            public string TransactionId { get; set; }
        }

        public class Response
        {
            public Header Header { get; set; }
            public Records Records { get; set; }
        }

        public class Records
        {
            public List<Person> Record { get; set; }
        }

        public class QuodScore
        {
            public int Score { get; set; }
            public string Segment { get; set; }
            public int PaymentCommitmentScore { get; set; }
            public int ProfileScore { get; set; }
        }

        public class Apontamento
        {
            public string CNPJ { get; set; }
            public string CompanyName { get; set; }
            public string Nature { get; set; }
            public string Amount { get; set; }
            public string ContractNumber { get; set; }
            public Date DateOccurred { get; set; }
            public string ParticipantType { get; set; }
            public string ApontamentoStatus { get; set; }
            public Date DateIncluded { get; set; } 
            
            public Address Address { get; set; }
        }

        public class Apontamentos
        {
            public List<Apontamento> Apontamento { get; set; }
        }

        public class Negative
        {
            public Date DateLastApontamento { get; set; }
            public decimal PendenciesControlCred { get; set; }
            public Apontamentos Apontamentos { get; set; }
        }

        public class CcfApontamento
        {
            public string EntityType { get; set; }
            public string CpfCnpj { get; set; }
            public string ReportingCodeBank { get; set; }
            public string ReportingNameBank { get; set; }
            public string ReportingBranchPrefix { get; set; }
            public string ReasonBounce { get; set; }
            public string CountBounce { get; set; }
            public Date DateLastBounce { get; set; }
        }

        public class CcfApontamentos
        {
            public List<CcfApontamento> CcfApontamento { get; set; }
        }

        public class InquiryDetail
        {
            public Date DateInquiry { get; set; }
            public int InquiryCount { get; set; }
            public string Segment { get; set; }
        }

        public class InquiryDetails
        {
            public List<InquiryDetail> InquiryDetail { get; set; }
        }

        public class Inquiries
        {
            public int InquiryCountLast30Days { get; set; }
            public int InquiryCountLast31to60Days { get; set; }
            public int InquiryCountLast61to90Days { get; set; }
            public int InquiryCountMore90Days { get; set; }
            public InquiryDetails InquiryDetails { get; set; }
        }

        public class Protest
        {
            public string data_consulta { get; set; }
        }

        public class Protests
        {
            public List<Protest> Protest { get; set; }
        }

        public class Root
        {
            public QuodReportResponseEx QuodReportResponseEx { get; set; }
        }
    }
}
