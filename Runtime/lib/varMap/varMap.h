//
// Created by HP on 2022/7/23.
//

#ifndef RUNTIME_TEST_VARMAP_H
#define RUNTIME_TEST_VARMAP_H
#include "map"
#include "../PLC_Object.h"

class varMap {
public:
    /**
     * @brief ����ID�洢����Runtime�еı���������Ϣ���������͡�������ʵ��
     */
    std::map<int, PLC_Object*>* variableMap;

    /**
     * @brief ����ID�洢����Runtime�е�class��ʵ���ı�������map
     */
    std::map<int, std::map<int, PLC_Object*>*>* instanceVarMap;

    varMap(){
        variableMap = new std::map<int, PLC_Object*>();
        instanceVarMap= new std::map<int, std::map<int, PLC_Object*>*>();
    }

    /**
     * @brief ��runtime�ܶ��������Ӷ���
     * @param symbolID
     * @param object
     */
    void addVarToVarMap(int symbolID, PLC_Object* object) const{
        this->variableMap->emplace(symbolID, object);
    }

    /**
     * @brief ����ȡ���ܶ�����µĶ���
     * @tparam varType
     * @param symbolID
     * @return ����ID��Ӧ�����Ͷ���
     */
    template<typename varType>
    auto getSymbolByID(int symbolID){
        return dynamic_cast<varType>(this->variableMap->find(symbolID)->second);
    }

    /**
     * @brief ����ʵ��ID��ʵ���ڱ���ID��ȡ��ʵ����Ӧ�ı���
     * @tparam varType
     * @return ���ظ�������ID��Ӧ�Ķ���
     */
    template<typename varType>
    auto getInstanceVarByID(int instanceID, int varID){
        //��ȡʵ����Ӧ��map
        auto tempInstanceVarMap = this->instanceVarMap->find(instanceID)->second;
        //����Ӧ��map����ȡvarID��Ӧ�ı�������ת��Ȼ�󷵻�
        return dynamic_cast<varType>(tempInstanceVarMap->find(varID)->second);
    }

    /**
     * @brief ��ʵ�����Ŷ�Ӧ�����ñ����������뵽�ܱ��µ�instanceVarMap
     * @param instanceID ʵ����Ӧ��ID
     * @param classFieldMap ʵ����Runtime�����ɵ����ñ�������ָ��
     */
    void addClassFieldMapToInstanceMap(int instanceID, std::map<int, PLC_Object*>* classFieldMap){
        this->instanceVarMap->emplace(instanceID, classFieldMap);
    }
};


#endif //RUNTIME_TEST_VARMAP_H
