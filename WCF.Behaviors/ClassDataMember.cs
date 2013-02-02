using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WCF.Behaviors
{
    public class ClassDataMember
    {
        private readonly FieldInfo _fieldInfo = null;
        private readonly PropertyInfo _propertyInfo = null;

        private ClassDataMember(FieldInfo fieldInfo, PropertyInfo propertyInfo)
        {
            if (fieldInfo == null && propertyInfo == null)
                throw new ArgumentException();

            _fieldInfo = fieldInfo;
            _propertyInfo = propertyInfo;
        }

        public ClassDataMember(FieldInfo fieldInfo)
            : this(fieldInfo, null)
        {
        }

        public ClassDataMember(PropertyInfo propertyInfo)
            : this(null, propertyInfo)
        {   
        }

        public Type MemberType
        {
            get
            {
                return (_fieldInfo != null) ?
                    _fieldInfo.FieldType :
                    _propertyInfo.PropertyType;                    
            }
        }

        public string Name
        {
            get
            {
                return _fieldInfo != null ?
                    _fieldInfo.Name :
                    _propertyInfo.Name;
            }
        }

        public void SetValue(object instance, object value)
        {
            if (_fieldInfo != null)
            {
                _fieldInfo.SetValue(instance, value);
            }
            else
            {
                _propertyInfo.SetValue(instance, value);
            }

        }

        public object GetValue(object instance)
        {
            if (_fieldInfo != null)
            {
                return _fieldInfo.GetValue(instance);
            }
            else
            {
                return _propertyInfo.GetValue(instance);
            }
        }
    }
}
