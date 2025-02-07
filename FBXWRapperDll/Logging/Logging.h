/*
    File: Logging.h

    Win32 api console + txt file loggin

    Author: Phazer, 2020-2023    
*/

#pragma once

#include <Windows.h>
#include <fstream>
#include <string>
#include <iostream>
#include <fstream>
#include <locale>
#include <codecvt>
#include "..\Helpers\Tools.h"

#define FULL_FUNC_INFO(message) std::string(__func__) +  std::string(": Line: ") + std::to_string(__LINE__) + ": " + message
#define LogActionError(msg) ImplLog::LogActionErrorFalse( FULL_FUNC_INFO(msg) );

#define LogInfo(msg) ImplLog::LogActionInfo( FULL_FUNC_INFO(msg) );

#define LogAction(message)  ImplLog::LogActionInfo( \
	std::string(__func__) +  std::string(": Line: ") + std::to_string(__LINE__) + ": " + message);\

#define LogActionColor(message)  ImplLog::LogActionConcoleColor( \
	std::string(__func__) +  std::string(": Line: ") + std::to_string(__LINE__) + ": " + message);\

#define LogActionSuccess(message) ImplLog::LogAction_success(message);

#define LogActionWarning(message) ImplLog::LogAction_warning(message);

enum ConsoleColorFG
{
    FG_BLACK = 0,
    FG_DARKBLUE = FOREGROUND_BLUE,
    FG_DARKGREEN = FOREGROUND_GREEN,
    FG_DARKCYAN = FOREGROUND_GREEN | FOREGROUND_BLUE,
    FG_DARKRED = FOREGROUND_RED,
    FG_DARKMAGENTA = FOREGROUND_RED | FOREGROUND_BLUE,
    FG_DARKYELLOW = FOREGROUND_RED | FOREGROUND_GREEN,
    FG_DARKGRAY = FOREGROUND_RED | FOREGROUND_GREEN | FOREGROUND_BLUE,
    FG_GRAY = FOREGROUND_INTENSITY,
    FG_BLUE = FOREGROUND_INTENSITY | FOREGROUND_BLUE,
    FG_GREEN = FOREGROUND_INTENSITY | FOREGROUND_GREEN,
    FG_CYAN = FOREGROUND_INTENSITY | FOREGROUND_GREEN | FOREGROUND_BLUE,
    FG_RED = FOREGROUND_INTENSITY | FOREGROUND_RED,
    FG_MAGENTA = FOREGROUND_INTENSITY | FOREGROUND_RED | FOREGROUND_BLUE,
    FG_YELLOW = FOREGROUND_INTENSITY | FOREGROUND_RED | FOREGROUND_GREEN,
    FG_WHITE = FOREGROUND_INTENSITY | FOREGROUND_RED | FOREGROUND_GREEN | FOREGROUND_BLUE,
};

enum ConsoleColorBG
{
    BG_BLACK = 0,
    BG_DARKBLUE = BACKGROUND_BLUE,
    BG_DARKGREEN = BACKGROUND_GREEN,
    BG_DARKCYAN = BACKGROUND_GREEN | BACKGROUND_BLUE,
    BG_DARKRED = BACKGROUND_RED,
    BG_DARKMAGENTA = BACKGROUND_RED | BACKGROUND_BLUE,
    BG_DARKYELLOW = BACKGROUND_RED | BACKGROUND_GREEN,
    BG_DARKGRAY = BACKGROUND_RED | BACKGROUND_GREEN | BACKGROUND_BLUE,
    BG_GRAY = BACKGROUND_INTENSITY,
    BG_BLUE = BACKGROUND_INTENSITY | BACKGROUND_BLUE,
    BG_GREEN = BACKGROUND_INTENSITY | BACKGROUND_GREEN,
    BG_CYAN = BACKGROUND_INTENSITY | BACKGROUND_GREEN | BACKGROUND_BLUE,
    BG_RED = BACKGROUND_INTENSITY | BACKGROUND_RED,
    BG_MAGENTA = BACKGROUND_INTENSITY | BACKGROUND_RED | BACKGROUND_BLUE,
    BG_YELLOW = BACKGROUND_INTENSITY | BACKGROUND_RED | BACKGROUND_GREEN,
    BG_WHITE = BACKGROUND_INTENSITY | BACKGROUND_RED | BACKGROUND_GREEN | BACKGROUND_BLUE,
};

class ImplLog
{
public:
    static void LogActionInfo(const std::string& _strMsg);
    static void LogActionConcoleColor(const std::string& _strMsg, WORD wColorFlags = BG_MAGENTA | FG_WHITE);
    static void LogAction_success(const std::string& _strMsg);
    static bool LogActionErrorFalse(const std::string& _strMsg);
    static bool LogAction_warning(const std::string& _strMsg);
    static void LogWrite(const std::string& _strMsg);

    static void WriteToLogFile(const std::string& logString);
};

//namespace ConsoleForeground
//{
//    enum {
//        BLACK = 0,
//        DARKBLUE = FOREGROUND_BLUE,
//        DARKGREEN = FOREGROUND_GREEN,
//        DARKCYAN = FOREGROUND_GREEN | FOREGROUND_BLUE,
//        DARKRED = FOREGROUND_RED,
//        DARKMAGENTA = FOREGROUND_RED | FOREGROUND_BLUE,
//        DARKYELLOW = FOREGROUND_RED | FOREGROUND_GREEN,
//        DARKGRAY = FOREGROUND_RED | FOREGROUND_GREEN | FOREGROUND_BLUE,
//        GRAY = FOREGROUND_INTENSITY,
//        BLUE = FOREGROUND_INTENSITY | FOREGROUND_BLUE,
//        GREEN = FOREGROUND_INTENSITY | FOREGROUND_GREEN,
//        CYAN = FOREGROUND_INTENSITY | FOREGROUND_GREEN | FOREGROUND_BLUE,
//        RED = FOREGROUND_INTENSITY | FOREGROUND_RED,
//        MAGENTA = FOREGROUND_INTENSITY | FOREGROUND_RED | FOREGROUND_BLUE,
//        YELLOW = FOREGROUND_INTENSITY | FOREGROUND_RED | FOREGROUND_GREEN,
//        WHITE = FOREGROUND_INTENSITY | FOREGROUND_RED | FOREGROUND_GREEN | FOREGROUND_BLUE,
//    };
//}
//
//namespace ConsoleBackground
//{
//    enum ConsoleBackground {
//        BLACK = 0,
//        DARKBLUE = BACKGROUND_BLUE,
//        DARKGREEN = BACKGROUND_GREEN,
//        DARKCYAN = BACKGROUND_GREEN | BACKGROUND_BLUE,
//        DARKRED = BACKGROUND_RED,
//        DARKMAGENTA = BACKGROUND_RED | BACKGROUND_BLUE,
//        DARKYELLOW = BACKGROUND_RED | BACKGROUND_GREEN,
//        DARKGRAY = BACKGROUND_RED | BACKGROUND_GREEN | BACKGROUND_BLUE,
//        GRAY = BACKGROUND_INTENSITY,
//        BLUE = BACKGROUND_INTENSITY | BACKGROUND_BLUE,
//        GREEN = BACKGROUND_INTENSITY | BACKGROUND_GREEN,
//        CYAN = BACKGROUND_INTENSITY | BACKGROUND_GREEN | BACKGROUND_BLUE,
//        RED = BACKGROUND_INTENSITY | BACKGROUND_RED,
//        MAGENTA = BACKGROUND_INTENSITY | BACKGROUND_RED | BACKGROUND_BLUE,
//        YELLOW = BACKGROUND_INTENSITY | BACKGROUND_RED | BACKGROUND_GREEN,
//        WHITE = BACKGROUND_INTENSITY | BACKGROUND_RED | BACKGROUND_GREEN | BACKGROUND_BLUE,
//    };
//};


