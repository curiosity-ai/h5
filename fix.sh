#!/bin/bash
sed -i '/if (node.Body != null)/,+8c\
            if (node.Body != null)\
            {\
                Visit(node.Body);\
            }' Next/H5.Compiler.Service.Next/JavascriptEmitter.cs
