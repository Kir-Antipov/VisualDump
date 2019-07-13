﻿using System;
using System.Text;
using VisualDump.ExtraTypes;
using System.Collections.Generic;

namespace VisualDump.HTMLProviders.DefaultProviders
{
    public class CyclicalReferenceHTMLProvider : HTMLProvider
    {
        public override string ToHTML(object Obj, Stack<object> CallStack, params object[] Args) => ToHTML<CyclicalReference>(Obj, CallStack, (reference, stack) =>
        {
            Type t = reference.CyclicalObject.GetType();
            return new StringBuilder()
                .Append("<div class='table-wrap'>")
                    .Append("<div class='title-box'>")
                        .Append("<h4 class='title-box__headline'>")
                            .AppendTypeName(t)
                        .Append("</h4>")
                        .Append("<a class='title-box__collapse'>")
                            .Append("<i class='icon-cycle'>")
                                .Append("<?xml version='1.0' encoding='iso-8859-1'?>")
                                .Append("<svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' version='1.1' id='Capa_1' x='0px' y='0px' width='12px' height='12px' viewBox='0 0 561 561' style='enable-background:new 0 0 561 561;' xml:space='preserve'>")
                                    .Append("<g id='loop'>")
                                        .Append("<path d='M280.5,76.5V0l-102,102l102,102v-76.5c84.15,0,153,68.85,153,153c0,25.5-7.65,51-17.85,71.4l38.25,38.25    C471.75,357,484.5,321.3,484.5,280.5C484.5,168.3,392.7,76.5,280.5,76.5z M280.5,433.5c-84.15,0-153-68.85-153-153    c0-25.5,7.65-51,17.85-71.4l-38.25-38.25C89.25,204,76.5,239.7,76.5,280.5c0,112.2,91.8,204,204,204V561l102-102l-102-102V433.5z' fill='#FFFFFF'/>")
                                    .Append("</g>")
                                .Append("</svg>")
                            .Append("</i>")
                        .Append("</a>")
                    .Append("</div>")
                    .Append("<table class='table'>")
                        .Append("<tbody>")
                            .Append("<tr>")
                                .Append("<th class='type'>")
                                    .Append(t.FullName)
                                .Append("</th>")
                            .Append("</tr>")
                        .Append("</tbody>")
                    .Append("</table>")
                .Append("</div>")
            .ToString();
        });
    }
}
