using System;
using System.Collections.Generic;
using XCZ.FormManagement;

namespace XCZ.DataSeeder
{
    public class FormSeeder
    {
        public Form BaseForm { get; set; }

        public List<FormField> FormFields { get; set; }

        public FormSeeder()
        {
            NewForm();
        }

        private void NewForm()
        {
            BaseForm = new Form(Guid.NewGuid(), null, "/api/app/book", "Book", "书籍", "书籍管理", false);
            NewFormFields(BaseForm.Id);
        }

        private void NewFormFields(Guid formId)
        {
            FormFields = new List<FormField>
            {
                new FormField(Guid.NewGuid())
                {
                    FormId=formId,
                    FieldType= "input",
                    DataType="string",
                    FieldName="Name",
                    Label="书名",
                    Placeholder="请输入书名",
                    IsRequired=true,
                    Span=11
                },
                new FormField(Guid.NewGuid())
                {
                    FormId=formId,
                    FieldType= "number",
                    DataType="int",
                    FieldName="price",
                    Label="价格",
                    Placeholder="请输入价格",
                    DefaultValue="0",
                    IsRequired=true,
                    Span=13
                }
            };
        }
    }
}
