

/* this ALWAYS GENERATED file contains the IIDs and CLSIDs */

/* link this file in with the server and any clients */


 /* File created by MIDL compiler version 8.01.0628 */
/* at Tue Jan 19 06:14:07 2038
 */
/* Compiler settings for CalculatorSvr.idl:
    Oicf, W1, Zp8, env=Win32 (32b run), target_arch=X86 8.01.0628 
    protocol : dce , ms_ext, c_ext, robust
    error checks: allocation ref bounds_check enum stub_data 
    VC __declspec() decoration level: 
         __declspec(uuid()), __declspec(selectany), __declspec(novtable)
         DECLSPEC_UUID(), MIDL_INTERFACE()
*/
/* @@MIDL_FILE_HEADING(  ) */



#ifdef __cplusplus
extern "C"{
#endif 


#include <rpc.h>
#include <rpcndr.h>

#ifdef _MIDL_USE_GUIDDEF_

#ifndef INITGUID
#define INITGUID
#include <guiddef.h>
#undef INITGUID
#else
#include <guiddef.h>
#endif

#define MIDL_DEFINE_GUID(type,name,l,w1,w2,b1,b2,b3,b4,b5,b6,b7,b8) \
        DEFINE_GUID(name,l,w1,w2,b1,b2,b3,b4,b5,b6,b7,b8)

#else // !_MIDL_USE_GUIDDEF_

#ifndef __IID_DEFINED__
#define __IID_DEFINED__

typedef struct _IID
{
    unsigned long x;
    unsigned short s1;
    unsigned short s2;
    unsigned char  c[8];
} IID;

#endif // __IID_DEFINED__

#ifndef CLSID_DEFINED
#define CLSID_DEFINED
typedef IID CLSID;
#endif // CLSID_DEFINED

#define MIDL_DEFINE_GUID(type,name,l,w1,w2,b1,b2,b3,b4,b5,b6,b7,b8) \
        EXTERN_C __declspec(selectany) const type name = {l,w1,w2,{b1,b2,b3,b4,b5,b6,b7,b8}}

#endif // !_MIDL_USE_GUIDDEF_

MIDL_DEFINE_GUID(IID, IID_ITrigCalculator,0xAF41A15F,0xFBD4,0x4807,0xA2,0x59,0x6B,0x60,0x89,0xFE,0x76,0xA4);


MIDL_DEFINE_GUID(IID, IID_ISimpleCalculator,0xAF4978FE,0x6C37,0x4D20,0x84,0xE5,0xA3,0xFF,0x31,0x9D,0xFD,0xE9);


MIDL_DEFINE_GUID(IID, LIBID_CalculatorSvrLib,0x3FCF7C29,0x8229,0x4DEC,0xB1,0x8C,0x9A,0x76,0x91,0x09,0xA8,0x8E);


MIDL_DEFINE_GUID(CLSID, CLSID_Calculator,0x9017AC43,0xC0D1,0x49CE,0x81,0x56,0xA3,0x7E,0x07,0x12,0x10,0x0B);

#undef MIDL_DEFINE_GUID

#ifdef __cplusplus
}
#endif



