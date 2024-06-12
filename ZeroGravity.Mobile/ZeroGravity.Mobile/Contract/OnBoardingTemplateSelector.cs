using Xamarin.Forms;
using ZeroGravity.Mobile.Proxies;

namespace ZeroGravity.Mobile.Contract
{
    public class OnBoardingTemplateSelector : DataTemplateSelector
    {
        private DataTemplate defaulttemplate;

        public DataTemplate DefaultTemplate
        {
            get { return defaulttemplate; }
            set { defaulttemplate = value; }
        }

        private DataTemplate _template2;

        public DataTemplate Template2
        {
            get { return _template2; }
            set { _template2 = value; }
        }

        private DataTemplate _template3;

        public DataTemplate Template3
        {
            get { return _template3; }
            set { _template3 = value; }
        }

        private DataTemplate _template4;

        public DataTemplate Template4
        {
            get { return _template4; }
            set { _template4 = value; }
        }

        private DataTemplate _template5;

        public DataTemplate Template5
        {
            get { return _template5; }
            set { _template5 = value; }
        }

        private DataTemplate _template6;

        public DataTemplate Template6
        {
            get { return _template6; }
            set { _template6 = value; }
        }

        private DataTemplate _template7;

        public DataTemplate Template7
        {
            get { return _template7; }
            set { _template7 = value; }
        }

        private DataTemplate _template8;

        public DataTemplate Template8
        {
            get { return _template8; }
            set { _template8 = value; }
        }

        private DataTemplate _template9;

        public DataTemplate Template9
        {
            get { return _template9; }
            set { _template9 = value; }
        }

        private DataTemplate _template10;

        public DataTemplate Template10
        {
            get { return _template10; }
            set { _template10 = value; }
        }

        private DataTemplate _template11;

        public DataTemplate Template11
        {
            get { return _template11; }
            set { _template11 = value; }
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            QuestionProxy qp = item as QuestionProxy;
            if (qp == null)
                return null;

            DataTemplate template = null;
            switch (qp.Template)
            {
                case Shared.Enums.QuestionTemplate.Template1:
                    template = DefaultTemplate;
                    break;

                case Shared.Enums.QuestionTemplate.Template2:
                    template = Template2;
                    break;

                case Shared.Enums.QuestionTemplate.Template3:
                    template = Template3;
                    break;

                case Shared.Enums.QuestionTemplate.Template4:
                    template = Template4;
                    break;

                case Shared.Enums.QuestionTemplate.Template5:
                    template = Template5;
                    break;

                case Shared.Enums.QuestionTemplate.Template6:
                    template = Template6;
                    break;

                case Shared.Enums.QuestionTemplate.Template7:
                    template = Template7;
                    break;

                case Shared.Enums.QuestionTemplate.Template8:
                    template = Template8;
                    break;

                case Shared.Enums.QuestionTemplate.Template9:
                    template = Template9;
                    break;

                case Shared.Enums.QuestionTemplate.Template10:
                    template = Template10;
                    break;

                case Shared.Enums.QuestionTemplate.Template11:
                    template = Template11;
                    break;

                default:
                    template = DefaultTemplate;
                    break;
            }
            return template;
        }
    }
}