#ifndef DEBUG_H
#define DEBUG_H

#include <iostream>
#include <string>
#include <sstream>

// ������Լ���
enum DebugLevel {
    DEBUG_NONE = 0,  // ������κε�����Ϣ
    DEBUG_INFO,      // �����ͨ��Ϣ
    DEBUG_WARNING,   // ���������Ϣ
    DEBUG_ERROR,     // ���������Ϣ
    DEBUG_ALL        // ���������Ϣ
};

// ���õ��Լ���
#ifndef DEBUG_LEVEL
#define DEBUG_LEVEL DEBUG_INFO
#endif

// ������Ժ�
#define DEBUG(level, ...) do { \
    if ((level) <= DEBUG_LEVEL) { \
        std::ostringstream oss; \
        oss << __VA_ARGS__; \
        std::cerr << "[" << __FILE__ << ":" << __LINE__ << "] " << oss.str() << std::endl; \
    } \
} while (0)

// ���岻ͬ����ĵ��Ժ�
#define DEBUG_INFO(...) DEBUG(DEBUG_INFO, __VA_ARGS__)
#define DEBUG_WARNING(...) DEBUG(DEBUG_WARNING, __VA_ARGS__)
#define DEBUG_ERROR(...) DEBUG(DEBUG_ERROR, __VA_ARGS__)

#endif // DEBUG_H