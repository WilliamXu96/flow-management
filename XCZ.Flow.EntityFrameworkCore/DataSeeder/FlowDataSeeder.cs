using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using XCZ.FlowManagement;
using XCZ.FormManagement;

namespace XCZ.DataSeeder
{
    public class FlowDataSeeder : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<BaseFlow, Guid> _baseflowRepository;
        private readonly IRepository<FlowNode, Guid> _nodeRepository;
        private readonly IRepository<FlowLine, Guid> _lineRepository;
        private readonly IRepository<LineForm, Guid> _lineFormRepository;
        private readonly IRepository<Form, Guid> _formRepository;
        private readonly IRepository<FormField, Guid> _formFieldRepository;

        public FlowDataSeeder(
             IRepository<BaseFlow, Guid> baseRep,
            IRepository<FlowNode, Guid> nodeRep,
            IRepository<FlowLine, Guid> lineRep,
            IRepository<LineForm, Guid> lineFormRep,
            IRepository<Form, Guid> formRepository,
            IRepository<FormField, Guid> formFieldRepository
            )
        {
            _baseflowRepository = baseRep;
            _nodeRepository = nodeRep;
            _lineRepository = lineRep;
            _lineFormRepository = lineFormRep;
            _formRepository = formRepository;
            _formFieldRepository = formFieldRepository;
        }

        public virtual async Task SeedAsync(DataSeedContext context)
        {
            await CreateForm();
        }

        private async Task CreateForm()
        {
            var formExist = await _formRepository.AnyAsync();
            if (formExist) return;
            var formData = new FormSeeder();
            await _formRepository.InsertAsync(formData.BaseForm);
            await _formFieldRepository.InsertManyAsync(formData.FormFields);
            await CreateFlow(formData.BaseForm.Id, formData.FormFields[1].Id);
        }

        private async Task CreateFlow(Guid formId, Guid fieldId)
        {
            var formExist = await _formRepository.AnyAsync();
            if (formExist) return;
            var flowData = new FlowSeeder(formId, fieldId);
            await _baseflowRepository.InsertAsync(flowData.Flow);
            await _nodeRepository.InsertManyAsync(flowData.Nodes);
            await _lineRepository.InsertManyAsync(flowData.Lines);
            await _lineFormRepository.InsertManyAsync(flowData.LineForms);
        }
    }
}
