using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dxniraq2u2018.Models
{
    public class Common
    {

        public enum Relationship
        {
            [Display(Name = "Father")]
            Father = 0,
            [Display(Name = "Mother")]
            Mother = 1,
            [Display(Name = "Friend")]
            Friend = 2
        };

        public enum GenderType
        {
            [Display(Name = "أنثى")]
            Female = 0,
            [Display(Name = "ذكر")]
            Male = 1,

        };

        public enum BlogPostType
        {
            [Display(Name = "مهارات العمل")]
            BusinessSkills = 1,
            [Display(Name = "تجارب منتجات")]
            ProductsTestimonials = 2,
            [Display(Name = "مقالات علمية")]
            Articles = 3,
            [Display(Name = "قصص نجاح")]
            SuccessStories = 4,
            [Display(Name = "كتب")]
            Books = 5,
            [Display(Name = "مرئيات")]
            Videos = 6,
            [Display(Name = "سمعيات")]
            Audios = 6
        };

        public enum FileTypes
        {
            [Display(Name = "صورة")]
            Image = 0,
            [Display(Name = "ملف")]
            File = 1,
            [Display(Name = "فديو")]
            Video = 2
        };

        public enum Rating
        {
            [Display(Name = "لايوجد تقييم")]
            Umspecified = 0,

            [Display(Name = "*")]
            star1 = 1,

            [Display(Name = "**")]
            star2 = 2,

            [Display(Name = "***")]
            star3 = 3,

            [Display(Name = "****")]
            star4 = 4,

            [Display(Name = "*****")]
            star5 = 5,
        };

        public enum CourseLevel
        {
            [Display(Name = "مبتدأ")]
            level1 = 1,

            [Display(Name = "متوسط")]
            level2 = 2,

            [Display(Name = "متقدم")]
            level3 = 3,

        };

        public enum ReceivedByRelationship
        {
            [Display(Name = "غير محدد")]
            Unspecified = 0,

            [Display(Name = "الدرجة الاولى")]
            FirstClassFamilyMember = 1,

            [Display(Name = "الدرجة الثانية")]
            SecondClassFamilyMember = 2,

            [Display(Name = "صديق")]
            Friend = 3,
        };

        public enum CommunityType
        {
            [Display(Name = "Blog")]
            Personal = 0,
            [Display(Name = "Community")]
            Community = 1,
            [Display(Name = "Group")]
            Group = 2
        };

        public enum SecurityLevel
        {
            [Display(Name = "AdminsOnly")]
            AdminsOnly = 0,
            [Display(Name = "RequiresAdminApproval")]
            RequiresAdminApproval = 1,
            [Display(Name = "Open")]
            Open = 2
        };

        public enum GroupPostType
        {
            [Display(Name = "Opprotunity")]
            Opprotunity = 0,
            [Display(Name = "QA")]
            QA = 1,
            [Display(Name = "Article")]
            Article = 2
        };

        public enum ReportType
        {
            [Display(Name = "None")]
            None = 0,
            [Display(Name = "NotRelated")]
            NotRelated = 1,
            [Display(Name = "Violation")]
            Violation = 2,
            [Display(Name = "HateSpeech")]
            HateSpeech = 3
        };

    }
}
