using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace st2c.PLCSymbolAndScope.PLCSymbols
{
   

    /// <summary>
    /// PLC引用变量类，继承自PLCVariable
    /// </summary>
    public class PLCRefVariable : PLCVariable
    {
        private PLCVariable _referredVariable;  // 被引用的变量
        private int _referredLevel = 0;        // 引用层级

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public PLCRefVariable() : base()
        {
            Sort = PLCModifierEnum.Sort.REF;  // 设置变量类型为引用
        }

        /// <summary>
        /// 获取被引用的变量
        /// </summary>
        public PLCVariable ReferredVariable
        {
            get => _referredVariable;
            set => _referredVariable = value;
        }

        /// <summary>
        /// 获取或设置引用层级
        /// </summary>
        public int ReferredLevel
        {
            get => _referredLevel;
            set => _referredLevel = value;
        }

        /// <summary>
        /// 将对象转换为JSON格式
        /// </summary>
        public new JObject ToStringJson()
        {
            var jsonObject = new JObject();

            // 序列化基础变量属性
            jsonObject["varSections"] = VarSections.ToString();
            jsonObject["accessModifier"] = AccessModifier.ToString();
            jsonObject["retainQualifiers"] = RetainQualifiers.ToString();
            jsonObject["ifConst"] = IfConst;
            jsonObject["symbolId"] = SymbolId;
            jsonObject["typeId"] = TypeId;
            jsonObject["name"] = Name;
            jsonObject["rowNum"] = RowNum;
            jsonObject["columnNum"] = ColumnNum;
            jsonObject["sort"] = Sort.ToString();
            jsonObject["runtimeName"] = RuntimeName;
            jsonObject["runtimeTypeName"] = RuntimeTypeName;

            // 序列化引用特有属性
            jsonObject["referredLevel"] = _referredLevel;
            jsonObject["referredVariable"] = _referredVariable?.Name;

            // 包装最终JSON对象
            var jsonSymbol = new JObject();
            jsonSymbol["PLCVariable"] = jsonObject;
            return jsonSymbol;
        }
    }
}
