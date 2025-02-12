
#pragma once
#include <fbxsdk.h>
#include <vector>
#include <map>
#include "..\Logging\Logging.h"

namespace wrapdll
{
    class FBXUnitHelper
    {
    public:
        static double GetFactorToMeters(fbxsdk::FbxScene* poFbxScene)
        {
            if (!poFbxScene)
            {
                LogActionError("Passed FbxScene == nullptr. Returning stardard factor");
                return 1.0;
            }

            return poFbxScene->GetGlobalSettings().GetSystemUnit().GetConversionFactorTo(::fbxsdk::FbxSystemUnit::m);
        }

        static std::string GetUnitAsString(fbxsdk::FbxScene* poFbxScene)
        {
            if (!poFbxScene)
            {
                LogActionError("Passed FbxScene == nullptr. Returning stardard empty string");
                return "";
            }

            return poFbxScene->GetGlobalSettings().GetSystemUnit().GetScaleFactorAsString_Plurial().Buffer();
        }
    };
}