using System;
using System.Collections.Generic;
using System.Text;

namespace NetBox.Terminal.Core
{
   public enum TokenType
   {
      Parameter,

      String
   }

   public class Token
   {
      public Token(TokenType type, int position, string value)
      {
         Type = type;
         Position = position;
         Value = value;
      }

      public TokenType Type { get; }
      public string Value { get; }
      public int Position { get; }

      public override string ToString()
      {
         return $"{Type}: '{Value}'@{Position}";
      }
   }

   public class StringTokenizer
   {
      private static readonly char[] ParamTrims = new char[] { '{', '}' };

      public StringTokenizer()
      {

      }

      public List<Token> Tokenise(string format)
      {
         string token = string.Empty;
         bool isParameter = false;
         var tokens = new List<Token>();
         int namedPos = 0;

         foreach (char ch in format)
         {
            switch (ch)
            {
               case '{':
                  if (token != string.Empty)
                  {
                     tokens.Add(CreateToken(isParameter, token, ref namedPos));
                  }
                  isParameter = true;
                  token = string.Empty;
                  token += ch;
                  break;
               case '}':
                  if (token != string.Empty)
                  {
                     token += ch;
                     tokens.Add(CreateToken(isParameter, token, ref namedPos));
                  }
                  token = string.Empty;
                  isParameter = false;
                  break;
               default:
                  token += ch;
                  break;
            }
         }

         if (token != string.Empty)
            tokens.Add(CreateToken(false, token, ref namedPos));

         return tokens;
      }

      private Token CreateToken(bool isParameter, string value, ref int namedPos)
      {
         TokenType type = isParameter ? TokenType.Parameter : TokenType.String;
         int position = -1;

         if (isParameter)
         {
            value = value.Trim(ParamTrims);
            position = namedPos++;
         }

         return new Token(type, position, value);
      }

   }
}
