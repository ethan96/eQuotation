using Advantech.Myadvantech.DataAccess;

namespace Advantech.PIS.Business
{
    public class ModelBusinessLogic
    {
         
        /// <summary>
        /// Get model by model ID
        /// </summary>
        /// <param name="model_id"></param>
        /// <returns></returns>
        public static Model GetModelByModelID(string model_id)
        {
            Model _model = new Model();
            string model_name = _model.GetModelNameByModelID(model_id);
            if (string.IsNullOrEmpty(model_name))
            {
                return _model;
            }
            _model.Model_Name = model_name;
            _model.LoadBasicModelInformation();
            return _model;
        }

        /// <summary>
        /// Get model by category ID and model ID
        /// </summary>
        /// <param name="category_id">The model belongs to which category</param>
        /// <param name="model_id"></param>
        /// <returns></returns>
        public static Model GetModelByCategory_ModelID(string category_id,string model_id)
        {
            Model _model = new Model();
            string model_name = _model.GetModelNameByCategory_ModelID(category_id, model_id);
            if (string.IsNullOrEmpty(model_name))
            {
                return _model;
            }
            _model.Model_Name = model_name;
            _model.LoadBasicModelInformation();
            return _model;
        }


        public static Model GetCompleteModelByModelID(string model_id)
        {
            Model _model = new Model();
            string model_name = _model.GetModelNameByModelID(model_id);
            if (string.IsNullOrEmpty(model_name))
            {
                return _model;
            }
            _model.Model_Name = model_name;
            _model.LoadCompleteModelInformation();
            return _model;
        }

        /// <summary>
        /// Get model by model Name
        /// </summary>
        /// <param name="model_name"></param>
        /// <returns></returns>
        public static Model GetModelByModelName(string model_name)
        {
            Model _model = new Model();
            if (string.IsNullOrEmpty(model_name))
            {
                return _model;
            }
            _model.Model_Name = model_name;
            _model.LoadBasicModelInformation();
            return _model;
        }

        /// <summary>
        /// Get model by model Name
        /// </summary>
        /// <param name="model_name"></param>
        /// <returns></returns>
        public static Model GetPublishByModelName(string model_name)
        {
            Model _model = new Model();
            if (string.IsNullOrEmpty(model_name))
            {
                return _model;
            }
            _model.Model_Name = model_name;
            _model.LoadModelPublish();
            return _model;
        }
    }
}
