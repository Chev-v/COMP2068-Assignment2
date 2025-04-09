[33mcommit 60746c62549867423a49554b5c0626b3d1bdc755[m[33m ([m[1;36mHEAD[m[33m -> [m[1;32mmain[m[33m)[m
Author: Chev-v <134878191+Chev-v@users.noreply.github.com>
Date:   Wed Apr 9 21:45:08 2025 -0400

    Fully functioning app
    
    The application now fully acts as it should, the edit, delete and update buttons are now all working. This also includes the image upload capabilities from Multer. So in conclusion all functionalities work and now it ready to start inplementing front end visuals to make the website cleaner and a more user friendly applicaiton.

[1mdiff --git a/app.js b/app.js[m
[1mindex ee4c5e3..9ec0075 100644[m
[1m--- a/app.js[m
[1m+++ b/app.js[m
[36m@@ -33,11 +33,18 @@[m [mhbs.registerHelper('formatDate', function (datetime) {[m
   return new Intl.DateTimeFormat('en-US', options).format(new Date(datetime)) + ' EST';[m
 });[m
 [m
[32m+[m[32mhbs.registerHelper('includes', function (str, value) {[m
[32m+[m[32m  return typeof str === 'string' && str.includes(value);[m
[32m+[m[32m});[m
[32m+[m
[32m+[m
 app.use(logger('dev'));[m
 app.use(express.json());[m
 app.use(express.urlencoded({ extended: false }));[m
 app.use(cookieParser());[m
 app.use(express.static(path.join(__dirname, 'public')));[m
[32m+[m[32mapp.use('/uploads', express.static(path.join(__dirname, 'uploads')));[m
[32m+[m
 [m
 app.use('/', indexRouter);[m
 app.use('/users', usersRouter);[m
[1mdiff --git a/models/Note.js b/models/Note.js[m
[1mindex a9f66e1..073c48a 100644[m
[1m--- a/models/Note.js[m
[1m+++ b/models/Note.js[m
[36m@@ -1,26 +1,11 @@[m
 const mongoose = require('mongoose');[m
 [m
 const noteSchema = new mongoose.Schema({[m
[31m-  title: {[m
[31m-    type: String,[m
[31m-    required: true[m
[31m-  },[m
[31m-  body: {[m
[31m-    type: String,[m
[31m-    required: true[m
[31m-  },[m
[31m-  createdAt: {[m
[31m-    type: Date,[m
[31m-    default: Date.now[m
[31m-  },[m
[31m-  file: {[m
[31m-    type: String, // file upload path (image/video)[m
[31m-    required: false[m
[31m-  },[m
[31m-  user: {[m
[31m-    type: mongoose.Schema.Types.ObjectId,[m
[31m-    ref: 'User' // we’ll set this up later for authentication[m
[31m-  }[m
[32m+[m[32m  title: { type: String, required: true },[m
[32m+[m[32m  body: { type: String, required: true },[m
[32m+[m[32m  createdAt: { type: Date, default: Date.now },[m
[32m+[m[32m  file: [String], // allow multiple files[m
[32m+[m[32m  user: { type: mongoose.Schema.Types.ObjectId, ref: 'User' }[m
 });[m
 [m
 module.exports = mongoose.model('Note', noteSchema);[m
[1mdiff --git a/node_modules/.bin/mkdirp b/node_modules/.bin/mkdirp[m
[1mnew file mode 100644[m
[1mindex 0000000..1ab9c81[m
[1m--- /dev/null[m
[1m+++ b/node_modules/.bin/mkdirp[m
[36m@@ -0,0 +1,16 @@[m
[32m+[m[32m#!/bin/sh[m
[32m+[m[32mbasedir=$(dirname "$(echo "$0" | sed -e 's,\\,/,g')")[m
[32m+[m
[32m+[m[32mcase `uname` in[m
[32m+[m[32m    *CYGWIN*|*MINGW*|*MSYS*)[m
[32m+[m[32m        if command -v cygpath > /dev/null 2>&1; then[m
[32m+[m[32m            basedir=`cygpath -w "$basedir"`[m
[32m+[m[32m        fi[m
[32m+[m[32m    ;;[m
[32m+[m[32mesac[m
[32m+[m
[32m+[m[32mif [ -x "$basedir/node" ]; then[m
[32m+[m[32m  exec "$basedir/node"  "$basedir/../mkdirp/bin/cmd.js" "$@"[m
[32m+[m[32melse[m[41m [m
[32m+[m[32m  exec node  "$basedir/../mkdirp/bin/cmd.js" "$@"[m
[32m+[m[32mfi[m
[1mdiff --git a/node_modules/.bin/mkdirp.cmd b/node_modules/.bin/mkdirp.cmd[m
[1mnew file mode 100644[m
[1mindex 0000000..a865dd9[m
[1m--- /dev/null[m
[1m+++ b/node_modules/.bin/mkdirp.cmd[m
[36m@@ -0,0 +1,17 @@[m
[32m+[m[32m@ECHO off[m
[32m+[m[32mGOTO start[m
[32m+[m[32m:find_dp0[m
[32m+[m[32mSET dp0=%~dp0[m
[32m+[m[32mEXIT /b[m
[32m+[m[32m:start[m
[32m+[m[32mSETLOCAL[m
[32m+[m[32mCALL :find_dp0[m
[32m+[m
[32m+[m[32mIF EXIST "%dp0%\node.exe" ([m
[32m+[m[32m  SET "_prog=%dp0%\node.exe"[m
[32m+[m[32m) ELSE ([m
[32m+[m[32m  SET "_prog=node"[m
[32m+[m[32m  SET PATHEXT=%PATHEXT:;.JS;=;%[m
[32m+[m[32m)[m
[32m+[m
[32m+[m[32mendLocal & goto #_undefined_# 2>NUL || title %COMSPEC% & "%_prog%"  "%dp0%\..\mkdirp\bin\cmd.js" %*[m
[1mdiff --git a/node_modules/.bin/mkdirp.ps1 b/node_modules/.bin/mkdirp.ps1[m
[1mnew file mode 100644[m
[1mindex 0000000..911e854[m
[1m--- /dev/null[m
[1m+++ b/node_modules/.bin/mkdirp.ps1[m
[36m@@ -0,0 +1,28 @@[m
[32m+[m[32m#!/usr/bin/env pwsh[m
[32m+[m[32m$basedir=Split-Path $MyInvocation.MyCommand.Definition -Parent[m
[32m+[m
[32m+[m[32m$exe=""[m
[32m+[m[32mif ($PSVersionTable.PSVersion -lt "6.0" -or $IsWindows) {[m
[32m+[m[32m  # Fix case when both the Windows and Linux builds of Node[m
[32m+[m[32m  # are installed in the same directory[m
[32m+[m[32m  $exe=".exe"[m
[32m+[m[32m}[m
[32m+[m[32m$ret=0[m
[32m+[m[32mif (Test-Path "$basedir/node$exe") {[m
[32m+[m[32m  # Support pipeline input[m
[32m+[m[32m  if ($MyInvocation.ExpectingInput) {[m
[32m+[m[32m    $input | & "$basedir/node$exe"  "$basedir/../mkdirp/bin/cmd.js" $args[m
[32m+[m[32m  } else {[m
[32m+[m[32m    & "$basedir/node$exe"  "$basedir/../mkdirp/bin/cmd.js" $args[m
[32m+[m[32m  }[m
[32m+[m[32m  $ret=$LASTEXITCODE[m
[32m+[m[32m} else {[m
[32m+[m[32m  # Support pipeline input[m
[32m+[m[32m  if ($MyInvocation.ExpectingInput) {[m
[32m+[m[32m    $input | & "node$exe"  "$basedir/../mkdirp/bin/cmd.js" $args[m
[32m+[m[32m  } else {[m
[32m+[m[32m    & "node$exe"  "$basedir/../mkdirp/bin/cmd.js" $args[m
[32m+[m[32m  }[m
[32m+[m[32m  $ret=$LASTEXITCODE[m
[32m+[m[32m}[m
[32m+[m[32mexit $ret[m
[1mdiff --git a/node_modules/.package-lock.json b/node_modules/.package-lock.json[m
[1mindex 785cea3..784b045 100644[m
[1m--- a/node_modules/.package-lock.json[m
[1m+++ b/node_modules/.package-lock.json[m
[36m@@ -41,6 +41,12 @@[m
         "node": ">= 0.6"[m
       }[m
     },[m
[32m+[m[32m    "node_modules/append-field": {[m
[32m+[m[32m      "version": "1.0.0",[m
[32m+[m[32m      "resolved": "https://registry.npmjs.org/append-field/-/append-field-1.0.0.tgz",[m
[32m+[m[32m      "integrity": "sha512-klpgFSWLW1ZEs8svjfb7g4qWY0YS5imI82dTg+QahUvJ8YqAY0P10Uk8tTyh9ZGuYEZEMaeJYCF5BFuX552hsw==",[m
[32m+[m[32m      "license": "MIT"[m
[32m+[m[32m    },[m
     "node_modules/array-flatten": {[m
       "version": "1.1.1",[m
       "resolved": "https://registry.npmjs.org/array-flatten/-/array-flatten-1.1.1.tgz",[m
[36m@@ -150,6 +156,23 @@[m
         "node": ">=16.20.1"[m
       }[m
     },[m
[32m+[m[32m    "node_modules/buffer-from": {[m
[32m+[m[32m      "version": "1.1.2",[m
[32m+[m[32m      "resolved": "https://registry.npmjs.org/buffer-from/-/buffer-from-1.1.2.tgz",[m
[32m+[m[32m      "integrity": "sha512-E+XQCRwSbaaiChtv6k6Dwgc+bx+Bs6vuKJHHl5kox/BaKbhiXzqQOwK4cO22yElGp2OCmjwVhT3HmxgyPGnJfQ==",[m
[32m+[m[32m      "license": "MIT"[m
[32m+[m[32m    },[m
[32m+[m[32m    "node_modules/busboy": {[m
[32m+[m[32m      "version": "1.6.0",[m
[32m+[m[32m      "resolved": "https://registry.npmjs.org/busboy/-/busboy-1.6.0.tgz",[m
[32m+[m[32m      "integrity": "sha512-8SFQbg/0hQ9xy3UNTB0YEnsNBbWfhf7RtnzpL7TkBiTBRfrQ9Fxcnz7VJsleJpyp6rVLvXiuORqjlHi5q+PYuA==",[m
[32m+[m[32m      "dependencies": {[m
[32m+[m[32m        "streamsearch": "^1.1.0"[m
[32m+[m[32m      },[m
[32m+[m[32m      "engines": {[m
[32m+[m[32m        "node": ">=10.16.0"[m
[32m+[m[32m      }[m
[32m+[m[32m    },[m
     "node_modules/bytes": {[m
       "version": "3.1.2",[m
       "resolved": "https://registry.npmjs.org/bytes/-/bytes-3.1.2.tgz",[m
[36m@@ -188,6 +211,21 @@[m
         "url": "https://github.com/sponsors/ljharb"[m
       }[m
     },[m
[32m+[m[32m    "node_modules/concat-stream": {[m
[32m+[m[32m      "version": "1.6.2",[m
[32m+[m[32m      "resolved": "https://registry.npmjs.org/concat-stream/-/concat-stream-1.6.2.tgz",[m
[32m+[m[32m      "integrity": "sha512-27HBghJxjiZtIk3Ycvn/4kbJk/1uZuJFfuPEns6LaEvpvG1f0hTea8lilrouyo9mVc2GWdcEZ8OLoGmSADlrCw==",[m
[32m+[m[32m      "engines": [[m
[32m+[m[32m        "node >= 0.8"[m
[32m+[m[32m      ],[m
[32m+[m[32m      "license": "MIT",[m
[32m+[m[32m      "dependencies": {[m
[32m+[m[32m        "buffer-from": "^1.0.0",[m
[32m+[m[32m        "inherits": "^2.0.3",[m
[32m+[m[32m        "readable-stream": "^2.2.2",[m
[32m+[m[32m        "typedarray": "^0.0.6"[m
[32m+[m[32m      }[m
[32m+[m[32m    },[m
     "node_modules/content-disposition": {[m
       "version": "0.5.4",[m
       "resolved": "https://registry.npmjs.org/content-disposition/-/content-disposition-0.5.4.tgz",[m
[36m@@ -257,6 +295,12 @@[m
       "integrity": "sha512-QADzlaHc8icV8I7vbaJXJwod9HWYp8uCqf1xa4OfNu1T7JVxQIrUgOWtHdNDtPiywmFbiS12VjotIXLrKM3orQ==",[m
       "license": "MIT"[m
     },[m
[32m+[m[32m    "node_modules/core-util-is": {[m
[32m+[m[32m      "version": "1.0.3",[m
[32m+[m[32m      "resolved": "https://registry.npmjs.org/core-util-is/-/core-util-is-1.0.3.tgz",[m
[32m+[m[32m      "integrity": "sha512-ZQBvi1DcpJ4GDqanjucZ2Hj3wEO5pZDS89BWbkcrvdxksJorwUDDZamX9ldFkp9aw2lmBDLgkObEA4DWNJ9FYQ==",[m
[32m+[m[32m      "license": "MIT"[m
[32m+[m[32m    },[m
     "node_modules/debug": {[m
       "version": "2.6.9",[m
       "resolved": "https://registry.npmjs.org/debug/-/debug-2.6.9.tgz",[m
[36m@@ -726,6 +770,12 @@[m
         "node": ">= 0.10"[m
       }[m
     },[m
[32m+[m[32m    "node_modules/isarray": {[m
[32m+[m[32m      "version": "1.0.0",[m
[32m+[m[32m      "resolved": "https://registry.npmjs.org/isarray/-/isarray-1.0.0.tgz",[m
[32m+[m[32m      "integrity": "sha512-VLghIWNM6ELQzo7zwmcg0NmTVyWKYjvIeM83yjp0wRDTmUnrM678fQbcKBo6n2CJEF0szoG//ytg+TKla89ALQ==",[m
[32m+[m[32m      "license": "MIT"[m
[32m+[m[32m    },[m
     "node_modules/kareem": {[m
       "version": "2.6.3",[m
       "resolved": "https://registry.npmjs.org/kareem/-/kareem-2.6.3.tgz",[m
[36m@@ -819,6 +869,18 @@[m
         "url": "https://github.com/sponsors/ljharb"[m
       }[m
     },[m
[32m+[m[32m    "node_modules/mkdirp": {[m
[32m+[m[32m      "version": "0.5.6",[m
[32m+[m[32m      "resolved": "https://registry.npmjs.org/mkdirp/-/mkdirp-0.5.6.tgz",[m
[32m+[m[32m      "integrity": "sha512-FP+p8RB8OWpF3YZBCrP5gtADmtXApB5AMLn+vdyA+PyxCjrCs00mjyUozssO33cwDeT3wNGdLxJ5M//YqtHAJw==",[m
[32m+[m[32m      "license": "MIT",[m
[32m+[m[32m      "dependencies": {[m
[32m+[m[32m        "minimist": "^1.2.6"[m
[32m+[m[32m      },[m
[32m+[m[32m      "bin": {[m
[32m+[m[32m        "mkdirp": "bin/cmd.js"[m
[32m+[m[32m      }[m
[32m+[m[32m    },[m
     "node_modules/mongodb": {[m
       "version": "6.15.0",[m
       "resolved": "https://registry.npmjs.org/mongodb/-/mongodb-6.15.0.tgz",[m
[36m@@ -969,6 +1031,24 @@[m
       "integrity": "sha512-Tpp60P6IUJDTuOq/5Z8cdskzJujfwqfOTkrwIwj7IRISpnkJnT6SyJ4PCPnGMoFjC9ddhal5KVIYtAt97ix05A==",[m
       "license": "MIT"[m
     },[m
[32m+[m[32m    "node_modules/multer": {[m
[32m+[m[32m      "version": "1.4.5-lts.2",[m
[32m+[m[32m      "resolved": "https://registry.npmjs.org/multer/-/multer-1.4.5-lts.2.tgz",[m
[32m+[m[32m      "integrity": "sha512-VzGiVigcG9zUAoCNU+xShztrlr1auZOlurXynNvO9GiWD1/mTBbUljOKY+qMeazBqXgRnjzeEgJI/wyjJUHg9A==",[m
[32m+[m[32m      "license": "MIT",[m
[32m+[m[32m      "dependencies": {[m
[32m+[m[32m        "append-field": "^1.0.0",[m
[32m+[m[32m        "busboy": "^1.0.0",[m
[32m+[m[32m        "concat-stream": "^1.5.2",[m
[32m+[m[32m        "mkdirp": "^0.5.4",[m
[32m+[m[32m        "object-assign": "^4.1.1",[m
[32m+[m[32m        "type-is": "^1.6.4",[m
[32m+[m[32m        "xtend": "^4.0.0"[m
[32m+[m[32m      },[m
[32m+[m[32m      "engines": {[m
[32m+[m[32m        "node": ">= 6.0.0"[m
[32m+[m[32m      }[m
[32m+[m[32m    },[m
     "node_modules/negotiator": {[m
       "version": "0.6.3",[m
       "resolved": "https://registry.npmjs.org/negotiator/-/negotiator-0.6.3.tgz",[m
[36m@@ -984,6 +1064,15 @@[m
       "integrity": "sha512-Yd3UES5mWCSqR+qNT93S3UoYUkqAZ9lLg8a7g9rimsWmYGK8cVToA4/sF3RrshdyV3sAGMXVUmpMYOw+dLpOuw==",[m
       "license": "MIT"[m
     },[m
[32m+[m[32m    "node_modules/object-assign": {[m
[32m+[m[32m      "version": "4.1.1",[m
[32m+[m[32m      "resolved": "https://registry.npmjs.org/object-assign/-/object-assign-4.1.1.tgz",[m
[32m+[m[32m      "integrity": "sha512-rJgTQnkUnH1sFw8yT6VSU3zD3sWmu6sZhIseY8VX+GRu3P6F7Fu+JNDoXfklElbLJSnc3FUQHVe4cU5hj+BcUg==",[m
[32m+[m[32m      "license": "MIT",[m
[32m+[m[32m      "engines": {[m
[32m+[m[32m        "node": ">=0.10.0"[m
[32m+[m[32m      }[m
[32m+[m[32m    },[m
     "node_modules/object-inspect": {[m
       "version": "1.13.4",[m
       "resolved": "https://registry.npmjs.org/object-inspect/-/object-inspect-1.13.4.tgz",[m
[36m@@ -1032,6 +1121,12 @@[m
       "integrity": "sha512-RA1GjUVMnvYFxuqovrEqZoxxW5NUZqbwKtYz/Tt7nXerk0LbLblQmrsgdeOxV5SFHf0UDggjS/bSeOZwt1pmEQ==",[m
       "license": "MIT"[m
     },[m
[32m+[m[32m    "node_modules/process-nextick-args": {[m
[32m+[m[32m      "version": "2.0.1",[m
[32m+[m[32m      "resolved": "https://registry.npmjs.org/process-nextick-args/-/process-nextick-args-2.0.1.tgz",[m
[32m+[m[32m      "integrity": "sha512-3ouUOpQhtgrbOa17J7+uxOTpITYWaGP7/AhoR3+A+/1e9skrzelGi/dXzEYyvbxubEF6Wn2ypscTKiKJFFn1ag==",[m
[32m+[m[32m      "license": "MIT"[m
[32m+[m[32m    },[m
     "node_modules/proxy-addr": {[m
       "version": "2.0.7",[m
       "resolved": "https://registry.npmjs.org/proxy-addr/-/proxy-addr-2.0.7.tgz",[m
[36m@@ -1139,6 +1234,21 @@[m
         "node": ">= 0.8"[m
       }[m
     },[m
[32m+[m[32m    "node_modules/readable-stream": {[m
[32m+[m[32m      "version": "2.3.8",[m
[32m+[m[32m      "resolved": "https://registry.npmjs.org/readable-stream/-/readable-stream-2.3.8.tgz",[m
[32m+[m[32m      "integrity": "sha512-8p0AUk4XODgIewSi0l8Epjs+EVnWiK7NoDIEGU0HhE7+ZyY8D1IMY7odu5lRrFXGg71L15KG8QrPmum45RTtdA==",[m
[32m+[m[32m      "license": "MIT",[m
[32m+[m[32m      "dependencies": {[m
[32m+[m[32m        "core-util-is": "~1.0.0",[m
[32m+[m[32m        "inherits": "~2.0.3",[m
[32m+[m[32m        "isarray": "~1.0.0",[m
[32m+[m[32m        "process-nextick-args": "~2.0.0",[m
[32m+[m[32m        "safe-buffer": "~5.1.1",[m
[32m+[m[32m        "string_decoder": "~1.1.1",[m
[32m+[m[32m        "util-deprecate": "~1.0.1"[m
[32m+[m[32m      }[m
[32m+[m[32m    },[m
     "node_modules/safe-buffer": {[m
       "version": "5.1.2",[m
       "resolved": "https://registry.npmjs.org/safe-buffer/-/safe-buffer-5.1.2.tgz",[m
[36m@@ -1374,6 +1484,23 @@[m
         "node": ">= 0.6"[m
       }[m
     },[m
[32m+[m[32m    "node_modules/streamsearch": {[m
[32m+[m[32m      "version": "1.1.0",[m
[32m+[m[32m      "resolved": "https://registry.npmjs.org/streamsearch/-/streamsearch-1.1.0.tgz",[m
[32m+[m[32m      "integrity": "sha512-Mcc5wHehp9aXz1ax6bZUyY5afg9u2rv5cqQI3mRrYkGC8rW2hM02jWuwjtL++LS5qinSyhj2QfLyNsuc+VsExg==",[m
[32m+[m[32m      "engines": {[m
[32m+[m[32m        "node": ">=10.0.0"[m
[32m+[m[32m      }[m
[32m+[m[32m    },[m
[32m+[m[32m    "node_modules/string_decoder": {[m
[32m+[m[32m      "version": "1.1.1",[m
[32m+[m[32m      "resolved": "https://registry.npmjs.org/string_decoder/-/string_decoder-1.1.1.tgz",[m
[32m+[m[32m      "integrity": "sha512-n/ShnvDi6FHbbVfviro+WojiFzv+s8MPMHBczVePfUpDJLwoLT0ht1l4YwBCbi8pJAveEEdnkHyPyTP/mzRfwg==",[m
[32m+[m[32m      "license": "MIT",[m
[32m+[m[32m      "dependencies": {[m
[32m+[m[32m        "safe-buffer": "~5.1.0"[m
[32m+[m[32m      }[m
[32m+[m[32m    },[m
     "node_modules/toidentifier": {[m
       "version": "1.0.1",[m
       "resolved": "https://registry.npmjs.org/toidentifier/-/toidentifier-1.0.1.tgz",[m
[36m@@ -1408,6 +1535,12 @@[m
         "node": ">= 0.6"[m
       }[m
     },[m
[32m+[m[32m    "node_modules/typedarray": {[m
[32m+[m[32m      "version": "0.0.6",[m
[32m+[m[32m      "resolved": "https://registry.npmjs.org/typedarray/-/typedarray-0.0.6.tgz",[m
[32m+[m[32m      "integrity": "sha512-/aCDEGatGvZ2BIk+HmLf4ifCJFwvKFNb9/JeZPMulfgFracn9QFcAf5GO8B/mweUjSoblS5In0cWhqpfs/5PQA==",[m
[32m+[m[32m      "license": "MIT"[m
[32m+[m[32m    },[m
     "node_modules/uglify-js": {[m
       "version": "3.19.3",[m
       "resolved": "https://registry.npmjs.org/uglify-js/-/uglify-js-3.19.3.tgz",[m
[36m@@ -1430,6 +1563,12 @@[m
         "node": ">= 0.8"[m
       }[m
     },[m
[32m+[m[32m    "node_modules/util-deprecate": {[m
[32m+[m[32m      "version": "1.0.2",[m
[32m+[m[32m      "resolved": "https://registry.npmjs.org/util-deprecate/-/util-deprecate-1.0.2.tgz",[m
[32m+[m[32m      "integrity": "sha512-EPD5q1uXyFxJpCrLnCc1nHnq3gOa6DZBocAIiI2TaSCA7VCJ1UJDMagCzIkXNsUYfD1daK//LTEQ8xiIbrHtcw==",[m
[32m+[m[32m      "license": "MIT"[m
[32m+[m[32m    },[m
     "node_modules/utils-merge": {[m
       "version": "1.0.1",[m
       "resolved": "https://registry.npmjs.org/utils-merge/-/utils-merge-1.0.1.tgz",[m
[36m@@ -1484,6 +1623,15 @@[m
       "resolved": "https://registry.npmjs.org/wordwrap/-/wordwrap-1.0.0.tgz",[m
       "integrity": "sha512-gvVzJFlPycKc5dZN4yPkP8w7Dc37BtP1yczEneOb4uq34pXZcvrtRTmWV8W+Ume+XCxKgbjM+nevkyFPMybd4Q==",[m
       "license": "MIT"[m
[32m+[m[32m    },[m
[32m+[m[32m    "node_modules/xtend": {[m
[32m+[m[32m      "version": "4.0.2",[m
[32m+[m[32m      "resolved": "https://registry.npmjs.org/xtend/-/xtend-4.0.2.tgz",[m
[32m+[m[32m      "integrity": "sha512-LKYU1iAXJXUgAXn9URjiu+MWhyUXHsvfp7mcuYm9dSUKK0/CjtrUwFAxD82/mCWbtLsGjFIad0wIsod4zrTAEQ==",[m
[32m+[m[32m      "license": "MIT",[m
[32m+[m[32m      "engines": {[m
[32m+[m[32m        "node": ">=0.4"[m
[32m+[m[32m      }[m
     }[m
   }[m
 }[m
[1mdiff --git a/node_modules/append-field/.npmignore b/node_modules/append-field/.npmignore[m
[1mnew file mode 100644[m
[1mindex 0000000..c2658d7[m
[1m--- /dev/null[m
[1m+++ b/node_modules/append-field/.npmignore[m
[36m@@ -0,0 +1 @@[m
[32m+[m[32mnode_modules/[m
[1mdiff --git a/node_modules/append-field/LICENSE b/node_modules/append-field/LICENSE[m
[1mnew file mode 100644[m
[1mindex 0000000..14b1f89[m
[1m--- /dev/null[m
[1m+++ b/node_modules/append-field/LICENSE[m
[36m@@ -0,0 +1,21 @@[m
[32m+[m[32mThe MIT License (MIT)[m
[32m+[m
[32m+[m[32mCopyright (c) 2015 Linus Unnebäck[m
[32m+[m
[32m+[m[32mPermission is hereby granted, free of charge, to any person obtaining a copy[m
[32m+[m[32mof this software and associated documentation files (the "Software"), to deal[m
[32m+[m[32min the Software without restriction, including without limitation the rights[m
[32m+[m[32mto use, copy, modify, merge, publish, distribute, sublicense, and/or sell[m
[32m+[m[32mcopies of the Software, and to permit persons to whom the Software is[m
[32m+[m[32mfurnished to do so, subject to the following conditions:[m
[32m+[m
[32m+[m[32mThe above copyright notice and this permission notice shall be included in all[m
[32m+[m[32mcopies or substantial portions of the Software.[m
[32m+[m
[32m+[m[32mTHE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR[m
[32m+[m[32mIMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,[m
[32m+[m[32mFITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE[m
[32m+[m[32mAUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER[m
[32m+[m[32mLIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,[m
[32m+[m[32mOUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE[m
[32m+[m[32mSOFTWARE.[m
[1mdiff --git a/node_modules/append-field/README.md b/node_modules/append-field/README.md[m
[1mnew file mode 100644[m
[1mindex 0000000..62b901b[m
[1m--- /dev/null[m
[1m+++ b/node_modules/append-field/README.md[m
[36m@@ -0,0 +1,44 @@[m
[32m+[m[32m# `append-field`[m
[32m+[m
[32m+[m[32mA [W3C HTML JSON forms spec](http://www.w3.org/TR/html-json-forms/) compliant[m
[32m+[m[32mfield appender (for lack of a better name). Useful for people implementing[m
[32m+[m[32m`application/x-www-form-urlencoded` and `multipart/form-data` parsers.[m
[32m+[m
[32m+[m[32mIt works best on objects created with `Object.create(null)`. Otherwise it might[m
[32m+[m[32mconflict with variables from the prototype (e.g. `hasOwnProperty`).[m
[32m+[m
[32m+[m[32m## Installation[m
[32m+[m
[32m+[m[32m```sh[m
[32m+[m[32mnpm install --save append-field[m
[32m+[m[32m```[m
[32m+[m
[32m+[m[32m## Usage[m
[32m+[m
[32m+[m[32m```javascript[m
[32m+[m[32mvar appendField = require('append-field')[m
[32m+[m[32mvar obj = Object.create(null)[m
[32m+[m
[32m+[m[32mappendField(obj, 'pets[0][species]', 'Dahut')[m
[32m+[m[32mappendField(obj, 'pets[0][name]', 'Hypatia')[m
[32m+[m[32mappendField(obj, 'pets[1][species]', 'Felis Stultus')[m
[32m+[m[32mappendField(obj, 'pets[1][name]', 'Billie')[m
[32m+[m
[32m+[m[32mconsole.log(obj)[m
[32m+[m[32m```[m
[32m+[m
[32m+[m[32m```text[m
[32m+[m[32m{ pets:[m
[32m+[m[32m   [ { species: 'Dahut', name: 'Hypatia' },[m
[32m+[m[32m     { species: 'Felis Stultus', name: 'Billie' } ] }[m
[32m+[m[32m```[m
[32m+[m
[32m+[m[32m## API[m
[32m+[m
[32m+[m[32m### `appendField(store, key, value)`[m
[32m+[m
[32m+[m[32mAdds the field named `key` with the value `value` to the object `store`.[m
[32m+[m
[32m+[m[32m## License[m
[32m+[m
[32m+[m[32mMIT[m
[1mdiff --git a/node_modules/append-field/index.js b/node_modules/append-field/index.js[m
[1mnew file mode 100644[m
[1mindex 0000000..fc5acc8[m
[1m--- /dev/null[m
[1m+++ b/node_modules/append-field/index.js[m
[36m@@ -0,0 +1,12 @@[m
[32m+[m[32mvar parsePath = require('./lib/parse-path')[m
[32m+[m[32mvar setValue = require('./lib/set-value')[m
[32m+[m
[32m+[m[32mfunction appendField (store, key, value) {[m
[32m+[m[32m  var steps = parsePath(key)[m
[32m+[m
[32m+[m[32m  steps.reduce(function (context, step) {[m
[32m+[m[32m    return setValue(context, step, context[step.key], value)[m
[32m+[m[32m  }, store)[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32mmodule.exports = appendField[m
[1mdiff --git a/node_modules/append-field/lib/parse-path.js b/node_modules/append-field/lib/parse-path.js[m
[1mnew file mode 100644[m
[1mindex 0000000..31d6179[m
[1m--- /dev/null[m
[1m+++ b/node_modules/append-field/lib/parse-path.js[m
[36m@@ -0,0 +1,53 @@[m
[32m+[m[32mvar reFirstKey = /^[^\[]*/[m
[32m+[m[32mvar reDigitPath = /^\[(\d+)\]/[m
[32m+[m[32mvar reNormalPath = /^\[([^\]]+)\]/[m
[32m+[m
[32m+[m[32mfunction parsePath (key) {[m
[32m+[m[32m  function failure () {[m
[32m+[m[32m    return [{ type: 'object', key: key, last: true }][m
[32m+[m[32m  }[m
[32m+[m
[32m+[m[32m  var firstKey = reFirstKey.exec(key)[0][m
[32m+[m[32m  if (!firstKey) return failure()[m
[32m+[m
[32m+[m[32m  var len = key.length[m
[32m+[m[32m  var pos = firstKey.length[m
[32m+[m[32m  var tail = { type: 'object', key: firstKey }[m
[32m+[m[32m  var steps = [tail][m
[32m+[m
[32m+[m[32m  while (pos < len) {[m
[32m+[m[32m    var m[m
[32m+[m
[32m+[m[32m    if (key[pos] === '[' && key[pos + 1] === ']') {[m
[32m+[m[32m      pos += 2[m
[32m+[m[32m      tail.append = true[m
[32m+[m[32m      if (pos !== len) return failure()[m
[32m+[m[32m      continue[m
[32m+[m[32m    }[m
[32m+[m
[32m+[m[32m    m = reDigitPath.exec(key.substring(pos))[m
[32m+[m[32m    if (m !== null) {[m
[32m+[m[32m      pos += m[0].length[m
[32m+[m[32m      tail.nextType = 'array'[m
[32m+[m[32m      tail = { type: 'array', key: parseInt(m[1], 10) }[m
[32m+[m[32m      steps.push(tail)[m
[32m+[m[32m      continue[m
[32m+[m[32m    }[m
[32m+[m
[32m+[m[32m    m = reNormalPath.exec(key.substring(pos))[m
[32m+[m[32m    if (m !== null) {[m
[32m+[m[32m      pos += m[0].length[m
[32m+[m[32m      tail.nextType = 'object'[m
[32m+[m[32m      tail = { type: 'object', key: m[1] }[m
[32m+[m[32m      steps.push(tail)[m
[32m+[m[32m      continue[m
[32m+[m[32m    }[m
[32m+[m
[32m+[m[32m    return failure()[m
[32m+[m[32m  }[m
[32m+[m
[32m+[m[32m  tail.last = true[m
[32m+[m[32m  return steps[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32mmodule.exports = parsePath[m
[1mdiff --git a/node_modules/append-field/lib/set-value.js b/node_modules/append-field/lib/set-value.js[m
[1mnew file mode 100644[m
[1mindex 0000000..c15e873[m
[1m--- /dev/null[m
[1m+++ b/node_modules/append-field/lib/set-value.js[m
[36m@@ -0,0 +1,64 @@[m
[32m+[m[32mfunction valueType (value) {[m
[32m+[m[32m  if (value === undefined) return 'undefined'[m
[32m+[m[32m  if (Array.isArray(value)) return 'array'[m
[32m+[m[32m  if (typeof value === 'object') return 'object'[m
[32m+[m[32m  return 'scalar'[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32mfunction setLastValue (context, step, currentValue, entryValue) {[m
[32m+[m[32m  switch (valueType(currentValue)) {[m
[32m+[m[32m    case 'undefined':[m
[32m+[m[32m      if (step.append) {[m
[32m+[m[32m        context[step.key] = [entryValue][m
[32m+[m[32m      } else {[m
[32m+[m[32m        context[step.key] = entryValue[m
[32m+[m[32m      }[m
[32m+[m[32m      break[m
[32m+[m[32m    case 'array':[m
[32m+[m[32m      context[step.key].push(entryValue)[m
[32m+[m[32m      break[m
[32m+[m[32m    case 'object':[m
[32m+[m[32m      return setLastValue(currentValue, { type: 'object', key: '', last: true }, currentValue[''], entryValue)[m
[32m+[m[32m    case 'scalar':[m
[32m+[m[32m      context[step.key] = [context[step.key], entryValue][m
[32m+[m[32m      break[m
[32m+[m[32m  }[m
[32m+[m
[32m+[m[32m  return context[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32mfunction setValue (context, step, currentValue, entryValue) {[m
[32m+[m[32m  if (step.last) return setLastValue(context, step, currentValue, entryValue)[m
[32m+[m
[32m+[m[32m  var obj[m
[32m+[m[32m  switch (valueType(currentValue)) {[m
[32m+[m[32m    case 'undefined':[m
[32m+[m[32m      if (step.nextType === 'array') {[m
[32m+[m[32m        context[step.key] = [][m
[32m+[m[32m      } else {[m
[32m+[m[32m        context[step.key] = Object.create(null)[m
[32m+[m[32m      }[m
[32m+[m[32m      return context[step.key][m
[32m+[m[32m    case 'object':[m
[32m+[m[32m      return context[step.key][m
[32m+[m[32m    case 'array':[m
[32m+[m[32m      if (step.nextType === 'array') {[m
[32m+[m[32m        return currentValue[m
[32m+[m[32m      }[m
[32m+[m
[32m+[m[32m      obj = Object.create(null)[m
[32m+[m[32m      context[step.key] = obj[m
[32m+[m[32m      currentValue.forEach(function (item, i) {[m
[32m+[m[32m        if (item !== undefined) obj['' + i] = item[m
[32m+[m[32m      })[m
[32m+[m
[32m+[m[32m      return obj[m
[32m+[m[32m    case 'scalar':[m
[32m+[m[32m      obj = Object.create(null)[m
[32m+[m[32m      obj[''] = currentValue[m
[32m+[m[32m      context[step.key] = obj[m
[32m+[m[32m      return obj[m
[32m+[m[32m  }[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32mmodule.exports = setValue[m
[1mdiff --git a/node_modules/append-field/package.json b/node_modules/append-field/package.json[m
[1mnew file mode 100644[m
[1mindex 0000000..8d6e716[m
[1m--- /dev/null[m
[1m+++ b/node_modules/append-field/package.json[m
[36m@@ -0,0 +1,19 @@[m
[32m+[m[32m{[m
[32m+[m[32m  "name": "append-field",[m
[32m+[m[32m  "version": "1.0.0",[m
[32m+[m[32m  "license": "MIT",[m
[32m+[m[32m  "author": "Linus Unnebäck <linus@folkdatorn.se>",[m
[32m+[m[32m  "main": "index.js",[m
[32m+[m[32m  "devDependencies": {[m
[32m+[m[32m    "mocha": "^2.2.4",[m
[32m+[m[32m    "standard": "^6.0.5",[m
[32m+[m[32m    "testdata-w3c-json-form": "^0.2.0"[m
[32m+[m[32m  },[m
[32m+[m[32m  "scripts": {[m
[32m+[m[32m    "test": "standard && mocha"[m
[32m+[m[32m  },[m
[32m+[m[32m  "repository": {[m
[32m+[m[32m    "type": "git",[m
[32m+[m[32m    "url": "http://github.com/LinusU/node-append-field.git"[m
[32m+[m[32m  }[m
[32m+[m[32m}[m
[1mdiff --git a/node_modules/append-field/test/forms.js b/node_modules/append-field/test/forms.js[m
[1mnew file mode 100644[m
[1mindex 0000000..dd6fbc9[m
[1m--- /dev/null[m
[1m+++ b/node_modules/append-field/test/forms.js[m
[36m@@ -0,0 +1,19 @@[m
[32m+[m[32m/* eslint-env mocha */[m
[32m+[m
[32m+[m[32mvar assert = require('assert')[m
[32m+[m[32mvar appendField = require('../')[m
[32m+[m[32mvar testData = require('testdata-w3c-json-form')[m
[32m+[m
[32m+[m[32mdescribe('Append Field', function () {[m
[32m+[m[32m  for (var test of testData) {[m
[32m+[m[32m    it('handles ' + test.name, function () {[m
[32m+[m[32m      var store = Object.create(null)[m
[32m+[m
[32m+[m[32m      for (var field of test.fields) {[m
[32m+[m[32m        appendField(store, field.key, field.value)[m
[32m+[m[32m      }[m
[32m+[m
[32m+[m[32m      assert.deepEqual(store, test.expected)[m
[32m+[m[32m    })[m
[32m+[m[32m  }[m
[32m+[m[32m})[m
[1mdiff --git a/node_modules/buffer-from/LICENSE b/node_modules/buffer-from/LICENSE[m
[1mnew file mode 100644[m
[1mindex 0000000..e4bf1d6[m
[1m--- /dev/null[m
[1m+++ b/node_modules/buffer-from/LICENSE[m
[36m@@ -0,0 +1,21 @@[m
[32m+[m[32mMIT License[m
[32m+[m
[32m+[m[32mCopyright (c) 2016, 2018 Linus Unnebäck[m
[32m+[m
[32m+[m[32mPermission is hereby granted, free of charge, to any person obtaining a copy[m
[32m+[m[32mof this software and associated documentation files (the "Software"), to deal[m
[32m+[m[32min the Software without restriction, including without limitation the rights[m
[32m+[m[32mto use, copy, modify, merge, publish, distribute, sublicense, and/or sell[m
[32m+[m[32mcopies of the Software, and to permit persons to whom the Software is[m
[32m+[m[32mfurnished to do so, subject to the following conditions:[m
[32m+[m
[32m+[m[32mThe above copyright notice and this permission notice shall be included in all[m
[32m+[m[32mcopies or substantial portions of the Software.[m
[32m+[m
[32m+[m[32mTHE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR[m
[32m+[m[32mIMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,[m
[32m+[m[32mFITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE[m
[32m+[m[32mAUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER[m
[32m+[m[32mLIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,[m
[32m+[m[32mOUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE[m
[32m+[m[32mSOFTWARE.[m
[1mdiff --git a/node_modules/buffer-from/index.js b/node_modules/buffer-from/index.js[m
[1mnew file mode 100644[m
[1mindex 0000000..e1a58b5[m
[1m--- /dev/null[m
[1m+++ b/node_modules/buffer-from/index.js[m
[36m@@ -0,0 +1,72 @@[m
[32m+[m[32m/* eslint-disable node/no-deprecated-api */[m
[32m+[m
[32m+[m[32mvar toString = Object.prototype.toString[m
[32m+[m
[32m+[m[32mvar isModern = ([m
[32m+[m[32m  typeof Buffer !== 'undefined' &&[m
[32m+[m[32m  typeof Buffer.alloc === 'function' &&[m
[32m+[m[32m  typeof Buffer.allocUnsafe === 'function' &&[m
[32m+[m[32m  typeof Buffer.from === 'function'[m
[32m+[m[32m)[m
[32m+[m
[32m+[m[32mfunction isArrayBuffer (input) {[m
[32m+[m[32m  return toString.call(input).slice(8, -1) === 'ArrayBuffer'[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32mfunction fromArrayBuffer (obj, byteOffset, length) {[m
[32m+[m[32m  byteOffset >>>= 0[m
[32m+[m
[32m+[m[32m  var maxLength = obj.byteLength - byteOffset[m
[32m+[m
[32m+[m[32m  if (maxLength < 0) {[m
[32m+[m[32m    throw new RangeError("'offset' is out of bounds")[m
[32m+[m[32m  }[m
[32m+[m
[32m+[m[32m  if (length === undefined) {[m
[32m+[m[32m    length = maxLength[m
[32m+[m[32m  } else {[m
[32m+[m[32m    length >>>= 0[m
[32m+[m
[32m+[m[32m    if (length > maxLength) {[m
[32m+[m[32m      throw new RangeError("'length' is out of bounds")[m
[32m+[m[32m    }[m
[32m+[m[32m  }[m
[32m+[m
[32m+[m[32m  return isModern[m
[32m+[m[32m    ? Buffer.from(obj.slice(byteOffset, byteOffset + length))[m
[32m+[m[32m    : new Buffer(new Uint8Array(obj.slice(byteOffset, byteOffset + length)))[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32mfunction fromString (string, encoding) {[m
[32m+[m[32m  if (typeof encoding !== 'string' || encoding === '') {[m
[32m+[m[32m    encoding = 'utf8'[m
[32m+[m[32m  }[m
[32m+[m
[32m+[m[32m  if (!Buffer.isEncoding(encoding)) {[m
[32m+[m[32m    throw new TypeError('"encoding" must be a valid string encoding')[m
[32m+[m[32m  }[m
[32m+[m
[32m+[m[32m  return isModern[m
[32m+[m[32m    ? Buffer.from(string, encoding)[m
[32m+[m[32m    : new Buffer(string, encoding)[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32mfunction bufferFrom (value, encodingOrOffset, length) {[m
[32m+[m[32m  if (typeof value === 'number') {[m
[32m+[m[32m    throw new TypeError('"value" argument must not be a number')[m
[32m+[m[32m  }[m
[32m+[m
[32m+[m[32m  if (isArrayBuffer(value)) {[m
[32m+[m[32m    return fromArrayBuffer(value, encodingOrOffset, length)[m
[32m+[m[32m  }[m
[32m+[m
[32m+[m[32m  if (typeof value === 'string') {[m
[32m+[m[32m    return fromString(value, encodingOrOffset)[m
[32m+[m[32m  }[m
[32m+[m
[32m+[m[32m  return isModern[m
[32m+[m[32m    ? Buffer.from(value)[m
[32m+[m[32m    : new Buffer(value)[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32mmodule.exports = bufferFrom[m
[1mdiff --git a/node_modules/buffer-from/package.json b/node_modules/buffer-from/package.json[m
[1mnew file mode 100644[m
[1mindex 0000000..6ac5327[m
[1m--- /dev/null[m
[1m+++ b/node_modules/buffer-from/package.json[m
[36m@@ -0,0 +1,19 @@[m
[32m+[m[32m{[m
[32m+[m[32m  "name": "buffer-from",[m
[32m+[m[32m  "version": "1.1.2",[m
[32m+[m[32m  "license": "MIT",[m
[32m+[m[32m  "repository": "LinusU/buffer-from",[m
[32m+[m[32m  "files": [[m
[32m+[m[32m    "index.js"[m
[32m+[m[32m  ],[m
[32m+[m[32m  "scripts": {[m
[32m+[m[32m    "test": "standard && node test"[m
[32m+[m[32m  },[m
[32m+[m[32m  "devDependencies": {[m
[32m+[m[32m    "standard": "^12.0.1"[m
[32m+[m[32m  },[m
[32m+[m[32m  "keywords": [[m
[32m+[m[32m    "buffer",[m
[32m+[m[32m    "buffer from"[m
[32m+[m[32m  ][m
[32m+[m[32m}[m
[1mdiff --git a/node_modules/buffer-from/readme.md b/node_modules/buffer-from/readme.md[m
[1mnew file mode 100644[m
[1mindex 0000000..9880a55[m
[1m--- /dev/null[m
[1m+++ b/node_modules/buffer-from/readme.md[m
[36m@@ -0,0 +1,69 @@[m
[32m+[m[32m# Buffer From[m
[32m+[m
[32m+[m[32mA [ponyfill](https://ponyfill.com) for `Buffer.from`, uses native implementation if available.[m
[32m+[m
[32m+[m[32m## Installation[m
[32m+[m
[32m+[m[32m```sh[m
[32m+[m[32mnpm install --save buffer-from[m
[32m+[m[32m```[m
[32m+[m
[32m+[m[32m## Usage[m
[32m+[m
[32m+[m[32m```js[m
[32m+[m[32mconst bufferFrom = require('buffer-from')[m
[32m+[m
[32m+[m[32mconsole.log(bufferFrom([1, 2, 3, 4]))[m
[32m+[m[32m//=> <Buffer 01 02 03 04>[m
[32m+[m
[32m+[m[32mconst arr = new Uint8Array([1, 2, 3, 4])[m
[32m+[m[32mconsole.log(bufferFrom(arr.buffer, 1, 2))[m
[32m+[m[32m//=> <Buffer 02 03>[m
[32m+[m
[32m+[m[32mconsole.log(bufferFrom('test', 'utf8'))[m
[32m+[m[32m//=> <Buffer 74 65 73 74>[m
[32m+[m
[32m+[m[32mconst buf = bufferFrom('test')[m
[32m+[m[32mconsole.log(bufferFrom(buf))[m
[32m+[m[32m//=> <Buffer 74 65 73 74>[m
[32m+[m[32m```[m
[32m+[m
[32m+[m[32m## API[m
[32m+[m
[32m+[m[32m### bufferFrom(array)[m
[32m+[m
[32m+[m[32m- `array` &lt;Array&gt;[m
[32m+[m
[32m+[m[32mAllocates a new `Buffer` using an `array` of octets.[m
[32m+[m
[32m+[m[32m### bufferFrom(arrayBuffer[, byteOffset[, length]])[m
[32m+[m
[32m+[m[32m- `arrayBuffer` &lt;ArrayBuffer&gt; The `.buffer` property of a TypedArray or ArrayBuffer[m
[32m+[m[32m- `byteOffset` &lt;Integer&gt; Where to start copying from `arrayBuffer`. **Default:** `0`[m
[32m+[m[32m- `length` &lt;Integer&gt; How many bytes to copy from `arrayBuffer`. **Default:** `arrayBuffer.length - byteOffset`[m
[32m+[m
[32m+[m[32mWhen passed a reference to the `.buffer` property of a TypedArray instance, the[m
[32m+[m[32mnewly created `Buffer` will share the same allocated memory as the TypedArray.[m
[32m+[m
[32m+[m[32mThe optional `byteOffset` and `length` arguments specify a memory range within[m
[32m+[m[32mthe `arrayBuffer` that will be shared by the `Buffer`.[m
[32m+[m
[32m+[m[32m### bufferFrom(buffer)[m
[32m+[m
[32m+[m[32m- `buffer` &lt;Buffer&gt; An existing `Buffer` to copy data from[m
[32m+[m
[32m+[m[32mCopies the passed `buffer` data onto a new `Buffer` instance.[m
[32m+[m
[32m+[m[32m### bufferFrom(string[, encoding])[m
[32m+[m
[32m+[m[32m- `string` &lt;String&gt; A string to encode.[m
[32m+[m[32m- `encoding` &lt;String&gt; The encoding of `string`. **Default:** `'utf8'`[m
[32m+[m
[32m+[m[32mCreates a new `Buffer` containing the given JavaScript string `string`. If[m
[32m+[m[32mprovided, the `encoding` parameter identifies the character encoding of[m
[32m+[m[32m`string`.[m
[32m+[m
[32m+[m[32m## See also[m
[32m+[m
[32m+[m[32m- [buffer-alloc](https://github.com/LinusU/buffer-alloc) A ponyfill for `Buffer.alloc`[m
[32m+[m[32m- [buffer-alloc-unsafe](https://github.com/LinusU/buffer-alloc-unsafe) A ponyfill for `Buffer.allocUnsafe`[m
[1mdiff --git a/node_modules/busboy/.eslintrc.js b/node_modules/busboy/.eslintrc.js[m
[1mnew file mode 100644[m
[1mindex 0000000..be9311d[m
[1m--- /dev/null[m
[1m+++ b/node_modules/busboy/.eslintrc.js[m
[36m@@ -0,0 +1,5 @@[m
[32m+[m[32m'use strict';[m
[32m+[m
[32m+[m[32mmodule.exports = {[m
[32m+[m[32m  extends: '@mscdex/eslint-config',[m
[32m+[m[32m};[m
[1mdiff --git a/node_modules/busboy/.github/workflows/ci.yml b/node_modules/busboy/.github/workflows/ci.yml[m
[1mnew file mode 100644[m
[1mindex 0000000..799bae0[m
[1m--- /dev/null[m
[1m+++ b/node_modules/busboy/.github/workflows/ci.yml[m
[36m@@ -0,0 +1,24 @@[m
[32m+[m[32mname: CI[m
[32m+[m
[32m+[m[32mon:[m
[32m+[m[32m  pull_request:[m
[32m+[m[32m  push:[m
[32m+[m[32m    branches: [ master ][m
[32m+[m
[32m+[m[32mjobs:[m
[32m+[m[32m  tests-linux:[m
[32m+[m[32m    runs-on: ubuntu-latest[m
[32m+[m[32m    strategy:[m
[32m+[m[32m      fail-fast: false[m
[32m+[m[32m      matrix:[m
[32m+[m[32m        node-version: [10.16.0, 10.x, 12.x, 14.x, 16.x][m
[32m+[m[32m    steps:[m
[32m+[m[32m      - uses: actions/checkout@v2[m
[32m+[m[32m      - name: Use Node.js ${{ matrix.node-version }}[m
[32m+[m[32m        uses: actions/setup-node@v1[m
[32m+[m[32m        with:[m
[32m+[m[32m          node-version: ${{ matrix.node-version }}[m
[32m+[m[32m      - name: Install module[m
[32m+[m[32m        run: npm install[m
[32m+[m[32m      - name: Run tests[m
[32m+[m[32m        run: npm test[m
[1mdiff --git a/node_modules/busboy/.github/workflows/lint.yml b/node_modules/busboy/.github/workflows/lint.yml[m
[1mnew file mode 100644[m
[1mindex 0000000..9f9e1f5[m
[1m--- /dev/null[m
[1m+++ b/node_modules/busboy/.github/workflows/lint.yml[m
[36m@@ -0,0 +1,23 @@[m
[32m+[m[32mname: lint[m
[32m+[m
[32m+[m[32mon:[m
[32m+[m[32m  pull_request:[m
[32m+[m[32m  push:[m
[32m+[m[32m    branches: [ master ][m
[32m+[m
[32m+[m[32menv:[m
[32m+[m[32m  NODE_VERSION: 16.x[m
[32m+[m
[32m+[m[32mjobs:[m
[32m+[m[32m  lint-js:[m
[32m+[m[32m    runs-on: ubuntu-latest[m
[32m+[m[32m    steps:[m
[32m+[m[32m      - uses: actions/checkout@v2[m
[32m+[m[32m      - name: Use Node.js ${{ env.NODE_VERSION }}[m
[32m+[m[32m        uses: actions/setup-node@v1[m
[32m+[m[32m        with:[m
[32m+[m[32m          node-version: ${{ env.NODE_VERSION }}[m
[32m+[m[32m      - name: Install ESLint + ESLint configs/plugins[m
[32m+[m[32m        run: npm install --only=dev[m
[32m+[m[32m      - name: Lint files[m
[32m+[m[32m        run: npm run lint[m
[1mdiff --git a/node_modules/busboy/LICENSE b/node_modules/busboy/LICENSE[m
[1mnew file mode 100644[m
[1mindex 0000000..290762e[m
[1m--- /dev/null[m
[1m+++ b/node_modules/busboy/LICENSE[m
[36m@@ -0,0 +1,19 @@[m
[32m+[m[32mCopyright Brian White. All rights reserved.[m
[32m+[m
[32m+[m[32mPermission is hereby granted, free of charge, to any person obtaining a copy[m
[32m+[m[32mof this software and associated documentation files (the "Software"), to[m
[32m+[m[32mdeal in the Software without restriction, including without limitation the[m
[32m+[m[32mrights to use, copy, modify, merge, publish, distribute, sublicense, and/or[m
[32m+[m[32msell copies of the Software, and to permit persons to whom the Software is[m
[32m+[m[32mfurnished to do so, subject to the following conditions:[m
[32m+[m
[32m+[m[32mThe above copyright notice and this permission notice shall be included in[m
[32m+[m[32mall copies or substantial portions of the Software.[m
[32m+[m
[32m+[m[32mTHE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR[m
[32m+[m[32mIMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,[m
[32m+[m[32mFITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE[m
[32m+[m[32mAUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER[m
[32m+[m[32mLIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING[m
[32m+[m[32mFROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS[m
[32m+[m[32mIN THE SOFTWARE.[m
\ No newline at end of file[m
[1mdiff --git a/node_modules/busboy/README.md b/node_modules/busboy/README.md[m
[1mnew file mode 100644[m
[1mindex 0000000..654af30[m
[1m--- /dev/null[m
[1m+++ b/node_modules/busboy/README.md[m
[36m@@ -0,0 +1,191 @@[m
[32m+[m[32m# Description[m
[32m+[m
[32m+[m[32mA node.js module for parsing incoming HTML form data.[m
[32m+[m
[32m+[m[32mChanges (breaking or otherwise) in v1.0.0 can be found [here](https://github.com/mscdex/busboy/issues/266).[m
[32m+[m
[32m+[m[32m# Requirements[m
[32m+[m
[32m+[m[32m* [node.js](http://nodejs.org/) -- v10.16.0 or newer[m
[32m+[m
[32m+[m
[32m+[m[32m# Install[m
[32m+[m
[32m+[m[32m    npm install busboy[m
[32m+[m
[32m+[m
[32m+[m[32m# Examples[m
[32m+[m
[32m+[m[32m* Parsing (multipart) with default options:[m
[32m+[m
[32m+[m[32m```js[m
[32m+[m[32mconst http = require('http');[m
[32m+[m
[32m+[m[32mconst busboy = require('busboy');[m
[32m+[m
[32m+[m[32mhttp.createServer((req, res) => {[m
[32m+[m[32m  if (req.method === 'POST') {[m
[32m+[m[32m    console.log('POST request');[m
[32m+[m[32m    const bb = busboy({ headers: req.headers });[m
[32m+[m[32m    bb.on('file', (name, file, info) => {[m
[32m+[m[32m      const { filename, encoding, mimeType } = info;[m
[32m+[m[32m      console.log([m
[32m+[m[32m        `File [${name}]: filename: %j, encoding: %j, mimeType: %j`,[m
[32m+[m[32m        filename,[m
[32m+[m[32m        encoding,[m
[32m+[m[32m        mimeType[m
[32m+[m[32m      );[m
[32m+[m[32m      file.on('data', (data) => {[m
[32m+[m[32m        console.log(`File [${name}] got ${data.length} bytes`);[m
[32m+[m[32m      }).on('close', () => {[m
[32m+[m[32m        console.log(`File [${name}] done`);[m
[32m+[m[32m      });[m
[32m+[m[32m    });[m
[32m+[m[32m    bb.on('field', (name, val, info) => {[m
[32m+[m[32m      console.log(`Field [${name}]: value: %j`, val);[m
[32m+[m[32m    });[m
[32m+[m[32m    bb.on('close', () => {[m
[32m+[m[32m      console.log('Done parsing form!');[m
[32m+[m[32m      res.writeHead(303, { Connection: 'close', Location: '/' });[m
[32m+[m[32m      res.end();[m
[32m+[m[32m    });[m
[32m+[m[32m    req.pipe(bb);[m
[32m+[m[32m  } else if (req.method === 'GET') {[m
[32m+[m[32m    res.writeHead(200, { Connection: 'close' });[m
[32m+[m[32m    res.end(`[m
[32m+[m[32m      <html>[m
[32m+[m[32m        <head></head>[m
[32m+[m[32m        <body>[m
[32m+[m[32m          <form method="POST" enctype="multipart/form-data">[m
[32m+[m[32m            <input type="file" name="filefield"><br />[m
[32m+[m[32m            <input type="text" name="textfield"><br />[m
[32m+[m[32m            <input type="submit">[m
[32m+[m[32m          </form>[m
[32m+[m[32m        </body>[m
[32m+[m[32m      </html>[m
[32m+[m[32m    `);[m
[32m+[m[32m  }[m
[32m+[m[32m}).listen(8000, () => {[m
[32m+[m[32m  console.log('Listening for requests');[m
[32m+[m[32m});[m
[32m+[m
[32m+[m[32m// Example output:[m
[32m+[m[32m//[m
[32m+[m[32m// Listening for requests[m
[32m+[m[32m//   < ... form submitted ... >[m
[32m+[m[32m// POST request[m
[32m+[m[32m// File [filefield]: filename: "logo.jpg", encoding: "binary", mime: "image/jpeg"[m
[32m+[m[32m// File [filefield] got 11912 bytes[m
[32m+[m[32m// Field [textfield]: value: "testing! :-)"[m
[32m+[m[32m// File [filefield] done[m
[32m+[m[32m// Done parsing form![m
[32m+[m[32m```[m
[32m+[m
[32m+[m[32m* Save all incoming files to disk:[m
[32m+[m
[32m+[m[32m```js[m
[32m+[m[32mconst { randomFillSync } = require('crypto');[m
[32m+[m[32mconst fs = require('fs');[m
[32m+[m[32mconst http = require('http');[m
[32m+[m[32mconst os = require('os');[m
[32m+[m[32mconst path = require('path');[m
[32m+[m
[32m+[m[32mconst busboy = require('busboy');[m
[32m+[m
[32m+[m[32mconst random = (() => {[m
[32m+[m[32m  const buf = Buffer.alloc(16);[m
[32m+[m[32m  return () => randomFillSync(buf).toString('hex');[m
[32m+[m[32m})();[m
[32m+[m
[32m+[m[32mhttp.createServer((req, res) => {[m
[32m+[m[32m  if (req.method === 'POST') {[m
[32m+[m[32m    const bb = busboy({ headers: req.headers });[m
[32m+[m[32m    bb.on('file', (name, file, info) => {[m
[32m+[m[32m      const saveTo = path.join(os.tmpdir(), `busboy-upload-${random()}`);[m
[32m+[m[32m      file.pipe(fs.createWriteStream(saveTo));[m
[32m+[m[32m    });[m
[32m+[m[32m    bb.on('close', () => {[m
[32m+[m[32m      res.writeHead(200, { 'Connection': 'close' });[m
[32m+[m[32m      res.end(`That's all folks!`);[m
[32m+[m[32m    });[m
[32m+[m[32m    req.pipe(bb);[m
[32m+[m[32m    return;[m
[32m+[m[32m  }[m
[32m+[m[32m  res.writeHead(404);[m
[32m+[m[32m  res.end();[m
[32m+[m[32m}).listen(8000, () => {[m
[32m+[m[32m  console.log('Listening for requests');[m
[32m+[m[32m});[m
[32m+[m[32m```[m
[32m+[m
[32m+[m
[32m+[m[32m# API[m
[32m+[m
[32m+[m[32m## Exports[m
[32m+[m
[32m+[m[32m`busboy` exports a single function:[m
[32m+[m
[32m+[m[32m**( _function_ )**(< _object_ >config) - Creates and returns a new _Writable_ form parser stream.[m
[32m+[m
[32m+[m[32m* Valid `config` properties:[m
[32m+[m
[32m+[m[32m    * **headers** - _object_ - These are the HTTP headers of the incoming request, which are used by individual parsers.[m
[32m+[m
[32m+[m[32m    * **highWaterMark** - _integer_ - highWaterMark to use for the parser stream. **Default:** node's _stream.Writable_ default.[m
[32m+[m
[32m+[m[32m    * **fileHwm** - _integer_ - highWaterMark to use for individual file streams. **Default:** node's _stream.Readable_ default.[m
[32m+[m
[32m+[m[32m    * **defCharset** - _string_ - Default character set to use when one isn't defined. **Default:** `'utf8'`.[m
[32m+[m
[32m+[m[32m    * **defParamCharset** - _string_ - For multipart forms, the default character set to use for values of part header parameters (e.g. filename) that are not extended parameters (that contain an explicit charset). **Default:** `'latin1'`.[m
[32m+[m
[32m+[m[32m    * **preservePath** - _boolean_ - If paths in filenames from file parts in a `'multipart/form-data'` request shall be preserved. **Default:** `false`.[m
[32m+[m
[32m+[m[32m    * **limits** - _object_ - Various limits on incoming data. Valid properties are:[m
[32m+[m
[32m+[m[32m        * **fieldNameSize** - _integer_ - Max field name size (in bytes). **Default:** `100`.[m
[32m+[m
[32m+[m[32m        * **fieldSize** - _integer_ - Max field value size (in bytes). **Default:** `1048576` (1MB).[m
[32m+[m
[32m+[m[32m        * **fields** - _integer_ - Max number of non-file fields. **Default:** `Infinity`.[m
[32m+[m
[32m+[m[32m        * **fileSize** - _integer_ - For multipart forms, the max file size (in bytes). **Default:** `Infinity`.[m
[32m+[m
[32m+[m[32m        * **files** - _integer_ - For multipart forms, the max number of file fields. **Default:** `Infinity`.[m
[32m+[m
[32m+[m[32m        * **parts** - _integer_ - For multipart forms, the max number of parts (fields + files). **Default:** `Infinity`.[m
[32m+[m
[32m+[m[32m        * **headerPairs** - _integer_ - For multipart forms, the max number of header key-value pairs to parse. **Default:** `2000` (same as node's http module).[m
[32m+[m
[32m+[m[32mThis function can throw exceptions if there is something wrong with the values in `config`. For example, if the Content-Type in `headers` is missing entirely, is not a supported type, or is missing the boundary for `'multipart/form-data'` requests.[m
[32m+[m
[32m+[m[32m## (Special) Parser stream events[m
[32m+[m
[32m+[m[32m* **file**(< _string_ >name, < _Readable_ >stream, < _object_ >info) - Emitted for each new file found. `name` contains the form field name. `stream` is a _Readable_ stream containing the file's data. No transformations/conversions (e.g. base64 to raw binary) are done on the file's data. `info` contains the following properties:[m
[32m+[m
[32m+[m[32m    * `filename` - _string_ - If supplied, this contains the file's filename. **WARNING:** You should almost _never_ use this value as-is (especially if you are using `preservePath: true` in your `config`) as it could contain malicious input. You are better off generating your own (safe) filenames, or at the very least using a hash of the filename.[m
[32m+[m
[32m+[m[32m    * `encoding` - _string_ - The file's `'Content-Transfer-Encoding'` value.[m
[32m+[m
[32m+[m[32m    * `mimeType` - _string_ - The file's `'Content-Type'` value.[m
[32m+[m
[32m+[m[32m    **Note:** If you listen for this event, you should always consume the `stream` whether you care about its contents or not (you can simply do `stream.resume();` if you want to discard/skip the contents), otherwise the `'finish'`/`'close'` event will never fire on the busboy parser stream.[m
[32m+[m[32m    However, if you aren't accepting files, you can either simply not listen for the `'file'` event at all or set `limits.files` to `0`, and any/all files will be automatically skipped (these skipped files will still count towards any configured `limits.files` and `limits.parts` limits though).[m
[32m+[m
[32m+[m[32m    **Note:** If a configured `limits.fileSize` limit was reached for a file, `stream` will both have a boolean property `truncated` set to `true` (best checked at the end of the stream) and emit a `'limit'` event to notify you when this happens.[m
[32m+[m
[32m+[m[32m* **field**(< _string_ >name, < _string_ >value, < _object_ >info) - Emitted for each new non-file field found. `name` contains the form field name. `value` contains the string value of the field. `info` contains the following properties:[m
[32m+[m
[32m+[m[32m    * `nameTruncated` - _boolean_ - Whether `name` was truncated or not (due to a configured `limits.fieldNameSize` limit)[m
[32m+[m
[32m+[m[32m    * `valueTruncated` - _boolean_ - Whether `value` was truncated or not (due to a configured `limits.fieldSize` limit)[m
[32m+[m
[32m+[m[32m    * `encoding` - _string_ - The field's `'Content-Transfer-Encoding'` value.[m
[32m+[m
[32m+[m[32m    * `mimeType` - _string_ - The field's `'Content-Type'` value.[m
[32m+[m
[32m+[m[32m* **partsLimit**() - Emitted when the configured `limits.parts` limit has been reached. No more `'file'` or `'field'` events will be emitted.[m
[32m+[m
[32m+[m[32m* **filesLimit**() - Emitted when the configured `limits.files` limit has been reached. No more `'file'` events will be emitted.[m
[32m+[m
[32m+[m[32m* **fieldsLimit**() - Emitted when the configured `limits.fields` limit has been reached. No more `'field'` events will be emitted.[m
[1mdiff --git a/node_modules/busboy/bench/bench-multipart-fields-100mb-big.js b/node_modules/busboy/bench/bench-multipart-fields-100mb-big.js[m
[1mnew file mode 100644[m
[1mindex 0000000..ef15729[m
[1m--- /dev/null[m
[1m+++ b/node_modules/busboy/bench/bench-multipart-fields-100mb-big.js[m
[36m@@ -0,0 +1,149 @@[m
[32m+[m[32m'use strict';[m
[32m+[m
[32m+[m[32mfunction createMultipartBuffers(boundary, sizes) {[m
[32m+[m[32m  const bufs = [];[m
[32m+[m[32m  for (let i = 0; i < sizes.length; ++i) {[m
[32m+[m[32m    const mb = sizes[i] * 1024 * 1024;[m
[32m+[m[32m    bufs.push(Buffer.from([[m
[32m+[m[32m      `--${boundary}`,[m
[32m+[m[32m      `content-disposition: form-data; name="field${i + 1}"`,[m
[32m+[m[32m      '',[m
[32m+[m[32m      '0'.repeat(mb),[m
[32m+[m[32m      '',[m
[32m+[m[32m    ].join('\r\n')));[m
[32m+[m[32m  }[m
[32m+[m[32m  bufs.push(Buffer.from([[m
[32m+[m[32m    `--${boundary}--`,[m
[32m+[m[32m    '',[m
[32m+[m[32m  ].join('\r\n')));[m
[32m+[m[32m  return bufs;[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32mconst boundary = '-----------------------------168072824752491622650073';[m
[32m+[m[32mconst buffers = createMultipartBuffers(boundary, [[m
[32m+[m[32m  10,[m
[32m+[m[32m  10,[m
[32m+[m[32m  10,[m
[32m+[m[32m  20,[m
[32m+[m[32m  50,[m
[32m+[m[32m]);[m
[32m+[m[32mconst calls = {[m
[32m+[m[32m  partBegin: 0,[m
[32m+[m[32m  headerField: 0,[m
[32m+[m[32m  headerValue: 0,[m
[32m+[m[32m  headerEnd: 0,[m
[32m+[m[32m  headersEnd: 0,[m
[32m+[m[32m  partData: 0,[m
[32m+[m[32m  partEnd: 0,[m
[32m+[m[32m  end: 0,[m
[32m+[m[32m};[m
[32m+[m
[32m+[m[32mconst moduleName = process.argv[2];[m
[32m+[m[32mswitch (moduleName) {[m
[32m+[m[32m  case 'busboy': {[m
[32m+[m[32m    const busboy = require('busboy');[m
[32m+[m
[32m+[m[32m    const parser = busboy({[m
[32m+[m[32m      limits: {[m
[32m+[m[32m        fieldSizeLimit: Infinity,[m
[32m+[m[32m      },[m
[32m+[m[32m      headers: {[m
[32m+[m[32m        'content-type': `multipart/form-data; boundary=${boundary}`,[m
[32m+[m[32m      },[m
[32m+[m[32m    });[m
[32m+[m[32m    parser.on('field', (name, val, info) => {[m
[32m+[m[32m      ++calls.partBegin;[m
[32m+[m[32m      ++calls.partData;[m
[32m+[m[32m      ++calls.partEnd;[m
[32m+[m[32m    }).on('close', () => {[m
[32m+[m[32m      ++calls.end;[m
[32m+[m[32m      console.timeEnd(moduleName);[m
[32m+[m[32m    });[m
[32m+[m
[32m+[m[32m    console.time(moduleName);[m
[32m+[m[32m    for (const buf of buffers)[m
[32m+[m[32m      parser.write(buf);[m
[32m+[m[32m    break;[m
[32m+[m[32m  }[m
[32m+[m
[32m+[m[32m  case 'formidable': {[m
[32m+[m[32m    const { MultipartParser } = require('formidable');[m
[32m+[m
[32m+[m[32m    const parser = new MultipartParser();[m
[32m+[m[32m    parser.initWithBoundary(boundary);[m
[32m+[m[32m    parser.on('data', ({ name }) => {[m
[32m+[m[32m      ++calls[name];[m
[32m+[m[32m      if (name === 'end')[m
[32m+[m[32m        console.timeEnd(moduleName);[m
[32m+[m[32m    });[m
[32m+[m
[32m+[m[32m    console.time(moduleName);[m
[32m+[m[32m    for (const buf of buffers)[m
[32m+[m[32m      parser.write(buf);[m
[32m+[m
[32m+[m[32m    break;[m
[32m+[m[32m  }[m
[32m+[m
[32m+[m[32m  case 'multiparty': {[m
[32m+[m[32m    const { Readable } = require('stream');[m
[32m+[m
[32m+[m[32m    const { Form } = require('multiparty');[m
[32m+[m
[32m+[m[32m    const form = new Form({[m
[32m+[m[32m      maxFieldsSize: Infinity,[m
[32m+[m[32m      maxFields: Infinity,[m
[32m+[m[32m      maxFilesSize: Infinity,[m
[32m+[m[32m      autoFields: false,[m
[32m+[m[32m      autoFiles: false,[m
[32m+[m[32m    });[m
[32m+[m
[32m+[m[32m    const req = new Readable({ read: () => {} });[m
[32m+[m[32m    req.headers = {[m
[32m+[m[32m      'content-type': `multipart/form-data; boundary=${boundary}`,[m
[32m+[m[32m    };[m
[32m+[m
[32m+[m[32m    function hijack(name, fn) {[m
[32m+[m[32m      const oldFn = form[name];[m
[32m+[m[32m      form[name] = function() {[m
[32m+[m[32m        fn();[m
[32m+[m[32m        return oldFn.apply(this, arguments);[m
[32m+[m[32m      };[m
[32m+[m[32m    }[m
[32m+[m
[32m+[m[32m    hijack('onParseHeaderField', () => {[m
[32m+[m[32m      ++calls.headerField;[m
[32m+[m[32m    });[m
[32m+[m[32m    hijack('onParseHeaderValue', () => {[m
[32m+[m[32m      ++calls.headerValue;[m
[32m+[m[32m    });[m
[32m+[m[32m    hijack('onParsePartBegin', () => {[m
[32m+[m[32m      ++calls.partBegin;[m
[32m+[m[32m    });[m
[32m+[m[32m    hijack('onParsePartData', () => {[m
[32m+[m[32m      ++calls.partData;[m
[32m+[m[32m    });[m
[32m+[m[32m    hijack('onParsePartEnd', () => {[m
[32m+[m[32m      ++calls.partEnd;[m
[32m+[m[32m    });[m
[32m+[m
[32m+[m[32m    form.on('close', () => {[m
[32m+[m[32m      ++calls.end;[m
[32m+[m[32m      console.timeEnd(moduleName);[m
[32m+[m[32m    }).on('part', (p) => p.resume());[m
[32m+[m
[32m+[m[32m    console.time(moduleName);[m
[32m+[m[32m    form.parse(req);[m
[32m+[m[32m    for (const buf of buffers)[m
[32m+[m[32m      req.push(buf);[m
[32m+[m[32m    req.push(null);[m
[32m+[m
[32m+[m[32m    break;[m
[32m+[m[32m  }[m
[32m+[m
[32m+[m[32m  default:[m
[32m+[m[32m    if (moduleName === undefined)[m
[32m+[m[32m      console.error('Missing parser module name');[m
[32m+[m[32m    else[m
[32m+[m[32m      console.error(`Invalid parser module name: ${moduleName}`);[m
[32m+[m[32m    process.exit(1);[m
[32m+[m[32m}[m
[1mdiff --git a/node_modules/busboy/bench/bench-multipart-fields-100mb-small.js b/node_modules/busboy/bench/bench-multipart-fields-100mb-small.js[m
[1mnew file mode 100644[m
[1mindex 0000000..f32d421[m
[1m--- /dev/null[m
[1m+++ b/node_modules/busboy/bench/bench-multipart-fields-100mb-small.js[m
[36m@@ -0,0 +1,143 @@[m
[32m+[m[32m'use strict';[m
[32m+[m
[32m+[m[32mfunction createMultipartBuffers(boundary, sizes) {[m
[32m+[m[32m  const bufs = [];[m
[32m+[m[32m  for (let i = 0; i < sizes.length; ++i) {[m
[32m+[m[32m    const mb = sizes[i] * 1024 * 1024;[m
[32m+[m[32m    bufs.push(Buffer.from([[m
[32m+[m[32m      `--${boundary}`,[m
[32m+[m[32m      `content-disposition: form-data; name="field${i + 1}"`,[m
[32m+[m[32m      '',[m
[32m+[m[32m      '0'.repeat(mb),[m
[32m+[m[32m      '',[m
[32m+[m[32m    ].join('\r\n')));[m
[32m+[m[32m  }[m
[32m+[m[32m  bufs.push(Buffer.from([[m
[32m+[m[32m    `--${boundary}--`,[m
[32m+[m[32m    '',[m
[32m+[m[32m  ].join('\r\n')));[m
[32m+[m[32m  return bufs;[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32mconst boundary = '-----------------------------168072824752491622650073';[m
[32m+[m[32mconst buffers = createMultipartBuffers(boundary, (new Array(100)).fill(1));[m
[32m+[m[32mconst calls = {[m
[32m+[m[32m  partBegin: 0,[m
[32m+[m[32m  headerField: 0,[m
[32m+[m[32m  headerValue: 0,[m
[32m+[m[32m  headerEnd: 0,[m
[32m+[m[32m  headersEnd: 0,[m
[32m+[m[32m  partData: 0,[m
[32m+[m[32m  partEnd: 0,[m
[32m+[m[32m  end: 0,[m
[32m+[m[32m};[m
[32m+[m
[32m+[m[32mconst moduleName = process.argv[2];[m
[32m+[m[32mswitch (moduleName) {[m
[32m+[m[32m  case 'busboy': {[m
[32m+[m[32m    const busboy = require('busboy');[m
[32m+[m
[32m+[m[32m    const parser = busboy({[m
[32m+[m[32m      limits: {[m
[32m+[m[32m        fieldSizeLimit: Infinity,[m
[32m+[m[32m      },[m
[32m+[m[32m      headers: {[m
[32m+[m[32m        'content-type': `multipart/form-data; boundary=${boundary}`,[m
[32m+[m[32m      },[m
[32m+[m[32m    });[m
[32m+[m[32m    parser.on('field', (name, val, info) => {[m
[32m+[m[32m      ++calls.partBegin;[m
[32m+[m[32m      ++calls.partData;[m
[32m+[m[32m      ++calls.partEnd;[m
[32m+[m[32m    }).on('close', () => {[m
[32m+[m[32m      ++calls.end;[m
[32m+[m[32m      console.timeEnd(moduleName);[m
[32m+[m[32m    });[m
[32m+[m
[32m+[m[32m    console.time(moduleName);[m
[32m+[m[32m    for (const buf of buffers)[m
[32m+[m[32m      parser.write(buf);[m
[32m+[m[32m    break;[m
[32m+[m[32m  }[m
[32m+[m
[32m+[m[32m  case 'formidable': {[m
[32m+[m[32m    const { MultipartParser } = require('formidable');[m
[32m+[m
[32m+[m[32m    const parser = new MultipartParser();[m
[32m+[m[32m    parser.initWithBoundary(boundary);[m
[32m+[m[32m    parser.on('data', ({ name }) => {[m
[32m+[m[32m      ++calls[name];[m
[32m+[m[32m      if (name === 'end')[m
[32m+[m[32m        console.timeEnd(moduleName);[m
[32m+[m[32m    });[m
[32m+[m
[32m+[m[32m    console.time(moduleName);[m
[32m+[m[32m    for (const buf of buffers)[m
[32m+[m[32m      parser.write(buf);[m
[32m+[m
[32m+[m[32m    break;[m
[32m+[m[32m  }[m
[32m+[m
[32m+[m[32m  case 'multiparty': {[m
[32m+[m[32m    const { Readable } = require('stream');[m
[32m+[m
[32m+[m[32m    const { Form } = require('multiparty');[m
[32m+[m
[32m+[m[32m    const form = new Form({[m
[32m+[m[32m      maxFieldsSize: Infinity,[m
[32m+[m[32m      maxFields: Infinity,[m
[32m+[m[32m      maxFilesSize: Infinity,[m
[32m+[m[32m      autoFields: false,[m
[32m+[m[32m      autoFiles: false,[m
[32m+[m[32m    });[m
[32m+[m
[32m+[m[32m    const req = new Readable({ read: () => {} });[m
[32m+[m[32m    req.headers = {[m
[32m+[m[32m      'content-type': `multipart/form-data; boundary=${boundary}`,[m
[32m+[m[32m    };[m
[32m+[m
[32m+[m[32m    function hijack(name, fn) {[m
[32m+[m[32m      const oldFn = form[name];[m
[32m+[m[32m      form[name] = function() {[m
[32m+[m[32m        fn();[m
[32m+[m[32m        return oldFn.apply(this, arguments);[m
[32m+[m[32m      };[m
[32m+[m[32m    }[m
[32m+[m
[32m+[m[32m    hijack('onParseHeaderField', () => {[m
[32m+[m[32m      ++calls.headerField;[m
[32m+[m[32m    });[m
[32m+[m[32m    hijack('onParseHeaderValue', () => {[m
[32m+[m[32m      ++calls.headerValue;[m
[32m+[m[32m    });[m
[32m+[m[32m    hijack('onParsePartBegin', () => {[m
[32m+[m[32m      ++calls.partBegin;[m
[32m+[m[32m    });[m
[32m+[m[32m    hijack('onParsePartData', () => {[m
[32m+[m[32m      ++calls.partData;[m
[32m+[m[32m    });[m
[32m+[m[32m    hijack('onParsePartEnd', () => {[m
[32m+[m[32m      ++calls.partEnd;[m
[32m+[m[32m    });[m
[32m+[m
[32m+[m[32m    form.on('close', () => {[m
[32m+[m[32m      ++calls.end;[m
[32m+[m[32m      console.timeEnd(moduleName);[m
[32m+[m[32m    }).on('part', (p) => p.resume());[m
[32m+[m
[32m+[m[32m    console.time(moduleName);[m
[32m+[m[32m    form.parse(req);[m
[32m+[m[32m    for (const buf of buffers)[m
[32m+[m[32m      req.push(buf);[m
[32m+[m[32m    req.push(null);[m
[32m+[m
[32m+[m[32m    break;[m
[32m+[m[32m  }[m
[32m+[m
[32m+[m[32m  default:[m
[32m+[m[32m    if (moduleName === undefined)[m
[32m+[m[32m      console.error('Missing parser module name');[m
[32m+[m[32m    else[m
[32m+[m[32m      console.error(`Invalid parser module name: ${moduleName}`);[m
[32m+[m[32m    process.exit(1);[m
[32m+[m[32m}[m
[1mdiff --git a/node_modules/busboy/bench/bench-multipart-files-100mb-big.js b/node_modules/busboy/bench/bench-multipart-files-100mb-big.js[m
[1mnew file mode 100644[m
[1mindex 0000000..b46bdee[m
[1m--- /dev/null[m
[1m+++ b/node_modules/busboy/bench/bench-multipart-files-100mb-big.js[m
[36m@@ -0,0 +1,154 @@[m
[32m+[m[32m'use strict';[m
[32m+[m
[32m+[m[32mfunction createMultipartBuffers(boundary, sizes) {[m
[32m+[m[32m  const bufs = [];[m
[32m+[m[32m  for (let i = 0; i < sizes.length; ++i) {[m
[32m+[m[32m    const mb = sizes[i] * 1024 * 1024;[m
[32m+[m[32m    bufs.push(Buffer.from([[m
[32m+[m[32m      `--${boundary}`,[m
[32m+[m[32m      `content-disposition: form-data; name="file${i + 1}"; `[m
[32m+[m[32m        + `filename="random${i + 1}.bin"`,[m
[32m+[m[32m      'content-type: application/octet-stream',[m
[32m+[m[32m      '',[m
[32m+[m[32m      '0'.repeat(mb),[m
[32m+[m[32m      '',[m
[32m+[m[32m    ].join('\r\n')));[m
[32m+[m[32m  }[m
[32m+[m[32m  bufs.push(Buffer.from([[m
[32m+[m[32m    `--${boundary}--`,[m
[32m+[m[32m    '',[m
[32m+[m[32m  ].join('\r\n')));[m
[32m+[m[32m  return bufs;[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32mconst boundary = '-----------------------------168072824752491622650073';[m
[32m+[m[32mconst buffers = createMultipartBuffers(boundary, [[m
[32m+[m[32m  10,[m
[32m+[m[32m  10,[m
[32m+[m[32m  10,[m
[32m+[m[32m  20,[m
[32m+[m[32m  50,[m
[32m+[m[32m]);[m
[32m+[m[32mconst calls = {[m
[32m+[m[32m  partBegin: 0,[m
[32m+[m[32m  headerField: 0,[m
[32m+[m[32m  headerValue: 0,[m
[32m+[m[32m  headerEnd: 0,[m
[32m+[m[32m  headersEnd: 0,[m
[32m+[m[32m  partData: 0,[m
[32m+[m[32m  partEnd: 0,[m
[32m+[m[32m  end: 0,[m
[32m+[m[32m};[m
[32m+[m
[32m+[m[32mconst moduleName = process.argv[2];[m
[32m+[m[32mswitch (moduleName) {[m
[32m+[m[32m  case 'busboy': {[m
[32m+[m[32m    const busboy = require('busboy');[m
[32m+[m
[32m+[m[32m    const parser = busboy({[m
[32m+[m[32m      limits: {[m
[32m+[m[32m        fieldSizeLimit: Infinity,[m
[32m+[m[32m      },[m
[32m+[m[32m      headers: {[m
[32m+[m[32m        'content-type': `multipart/form-data; boundary=${boundary}`,[m
[32m+[m[32m      },[m
[32m+[m[32m    });[m
[32m+[m[32m    parser.on('file', (name, stream, info) => {[m
[32m+[m[32m      ++calls.partBegin;[m
[32m+[m[32m      stream.on('data', (chunk) => {[m
[32m+[m[32m        ++calls.partData;[m
[32m+[m[32m      }).on('end', () => {[m
[32m+[m[32m        ++calls.partEnd;[m
[32m+[m[32m      });[m
[32m+[m[32m    }).on('close', () => {[m
[32m+[m[32m      ++calls.end;[m
[32m+[m[32m      console.timeEnd(moduleName);[m
[32m+[m[32m    });[m
[32m+[m
[32m+[m[32m    console.time(moduleName);[m
[32m+[m[32m    for (const buf of buffers)[m
[32m+[m[32m      parser.write(buf);[m
[32m+[m[32m    break;[m
[32m+[m[32m  }[m
[32m+[m
[32m+[m[32m  case 'formidable': {[m
[32m+[m[32m    const { MultipartParser } = require('formidable');[m
[32m+[m
[32m+[m[32m    const parser = new MultipartParser();[m
[32m+[m[32m    parser.initWithBoundary(boundary);[m
[32m+[m[32m    parser.on('data', ({ name }) => {[m
[32m+[m[32m      ++calls[name];[m
[32m+[m[32m      if (name === 'end')[m
[32m+[m[32m        console.timeEnd(moduleName);[m
[32m+[m[32m    });[m
[32m+[m
[32m+[m[32m    console.time(moduleName);[m
[32m+[m[32m    for (const buf of buffers)[m
[32m+[m[32m      parser.write(buf);[m
[32m+[m
[32m+[m[32m    break;[m
[32m+[m[32m  }[m
[32m+[m
[32m+[m[32m  case 'multiparty': {[m
[32m+[m[32m    const { Readable } = require('stream');[m
[32m+[m
[32m+[m[32m    const { Form } = require('multiparty');[m
[32m+[m
[32m+[m[32m    const form = new Form({[m
[32m+[m[32m      maxFieldsSize: Infinity,[m
[32m+[m[32m      maxFields: Infinity,[m
[32m+[m[32m      maxFilesSize: Infinity,[m
[32m+[m[32m      autoFields: false,[m
[32m+[m[32m      autoFiles: false,[m
[32m+[m[32m    });[m
[32m+[m
[32m+[m[32m    const req = new Readable({ read: () => {} });[m
[32m+[m[32m    req.headers = {[m
[32m+[m[32m      'content-type': `multipart/form-data; boundary=${boundary}`,[m
[32m+[m[32m    };[m
[32m+[m
[32m+[m[32m    function hijack(name, fn) {[m
[32m+[m[32m      const oldFn = form[name];[m
[32m+[m[32m      form[name] = function() {[m
[32m+[m[32m        fn();[m
[32m+[m[32m        return oldFn.apply(this, arguments);[m
[32m+[m[32m      };[m
[32m+[m[32m    }[m
[32m+[m
[32m+[m[32m    hijack('onParseHeaderField', () => {[m
[32m+[m[32m      ++calls.headerField;[m
[32m+[m[32m    });[m
[32m+[m[32m    hijack('onParseHeaderValue', () => {[m
[32m+[m[32m      ++calls.headerValue;[m
[32m+[m[32m    });[m
[32m+[m[32m    hijack('onParsePartBegin', () => {[m
[32m+[m[32m      ++calls.partBegin;[m
[32m+[m[32m    });[m
[32m+[m[32m    hijack('onParsePartData', () => {[m
[32m+[m[32m      ++calls.partData;[m
[32m+[m[32m    });[m
[32m+[m[32m    hijack('onParsePartEnd', () => {[m
[32m+[m[32m      ++calls.partEnd;[m
[32m+[m[32m    });[m
[32m+[m
[32m+[m[32m    form.on('close', () => {[m
[32m+[m[32m      ++calls.end;[m
[32m+[m[32m      console.timeEnd(moduleName);[m
[32m+[m[32m    }).on('part', (p) => p.resume());[m
[32m+[m
[32m+[m[32m    console.time(moduleName);[m
[32m+[m[32m    form.parse(req);[m
[32m+[m[32m    for (const buf of buffers)[m
[32m+[m[32m      req.push(buf);[m
[32m+[m[32m    req.push(null);[m
[32m+[m
[32m+[m[32m    break;[m
[32m+[m[32m  }[m
[32m+[m
[32m+[m[32m  default:[m
[32m+[m[32m    if (moduleName === undefined)[m
[32m+[m[32m      console.error('Missing parser module name');[m
[32m+[m[32m    else[m
[32m+[m[32m      console.error(`Invalid parser module name: ${moduleName}`);[m
[32m+[m[32m    process.exit(1);[m
[32m+[m[32m}[m
[1mdiff --git a/node_modules/busboy/bench/bench-multipart-files-100mb-small.js b/node_modules/busboy/bench/bench-multipart-files-100mb-small.js[m
[1mnew file mode 100644[m
[1mindex 0000000..46b5dff[m
[1m--- /dev/null[m
[1m+++ b/node_modules/busboy/bench/bench-multipart-files-100mb-small.js[m
[36m@@ -0,0 +1,148 @@[m
[32m+[m[32m'use strict';[m
[32m+[m
[32m+[m[32mfunction createMultipartBuffers(boundary, sizes) {[m
[32m+[m[32m  const bufs = [];[m
[32m+[m[32m  for (let i = 0; i < sizes.length; ++i) {[m
[32m+[m[32m    const mb = sizes[i] * 1024 * 1024;[m
[32m+[m[32m    bufs.push(Buffer.from([[m
[32m+[m[32m      `--${boundary}`,[m
[32m+[m[32m      `content-disposition: form-data; name="file${i + 1}"; `[m
[32m+[m[32m        + `filename="random${i + 1}.bin"`,[m
[32m+[m[32m      'content-type: application/octet-stream',[m
[32m+[m[32m      '',[m
[32m+[m[32m      '0'.repeat(mb),[m
[32m+[m[32m      '',[m
[32m+[m[32m    ].join('\r\n')));[m
[32m+[m[32m  }[m
[32m+[m[32m  bufs.push(Buffer.from([[m
[32m+[m[32m    `--${boundary}--`,[m
[32m+[m[32m    '',[m
[32m+[m[32m  ].join('\r\n')));[m
[32m+[m[32m  return bufs;[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32mconst boundary = '-----------------------------168072824752491622650073';[m
[32m+[m[32mconst buffers = createMultipartBuffers(boundary, (new Array(100)).fill(1));[m
[32m+[m[32mconst calls = {[m
[32m+[m[32m  partBegin: 0,[m
[32m+[m[32m  headerField: 0,[m
[32m+[m[32m  headerValue: 0,[m
[32m+[m[32m  headerEnd: 0,[m
[32m+[m[32m  headersEnd: 0,[m
[32m+[m[32m  partData: 0,[m
[32m+[m[32m  partEnd: 0,[m
[32m+[m[32m  end: 0,[m
[32m+[m[32m};[m
[32m+[m
[32m+[m[32mconst moduleName = process.argv[2];[m
[32m+[m[32mswitch (moduleName) {[m
[32m+[m[32m  case 'busboy': {[m
[32m+[m[32m    const busboy = require('busboy');[m
[32m+[m
[32m+[m[32m    const parser = busboy({[m
[32m+[m[32m      limits: {[m
[32m+[m[32m        fieldSizeLimit: Infinity,[m
[32m+[m[32m      },[m
[32m+[m[32m      headers: {[m
[32m+[m[32m        'content-type': `multipart/form-data; boundary=${boundary}`,[m
[32m+[m[32m      },[m
[32m+[m[32m    });[m
[32m+[m[32m    parser.on('file', (name, stream, info) => {[m
[32m+[m[32m      ++calls.partBegin;[m
[32m+[m[32m      stream.on('data', (chunk) => {[m
[32m+[m[32m        ++calls.partData;[m
[32m+[m[32m      }).on('end', () => {[m
[32m+[m[32m        ++calls.partEnd;[m
[32m+[m[32m      });[m
[32m+[m[32m    }).on('close', () => {[m
[32m+[m[32m      ++calls.end;[m
[32m+[m[32m      console.timeEnd(moduleName);[m
[32m+[m[32m    });[m
[32m+[m
[32m+[m[32m    console.time(moduleName);[m
[32m+[m[32m    for (const buf of buffers)[m
[32m+[m[32m      parser.write(buf);[m
[32m+[m[32m    break;[m
[32m+[m[32m  }[m
[32m+[m
[32m+[m[32m  case 'formidable': {[m
[32m+[m[32m    const { MultipartParser } = require('formidable');[m
[32m+[m
[32m+[m[32m    const parser = new MultipartParser();[m
[32m+[m[32m    parser.initWithBoundary(boundary);[m
[32m+[m[32m    parser.on('data', ({ name }) => {[m
[32m+[m[32m      ++calls[name];[m
[32m+[m[32m      if (name === 'end')[m
[32m+[m[32m        console.timeEnd(moduleName);[m
[32m+[m[32m    });[m
[32m+[m
[32m+[m[32m    console.time(moduleName);[m
[32m+[m[32m    for (const buf of buffers)[m
[32m+[m[32m      parser.write(buf);[m
[32m+[m
[32m+[m[32m    break;[m
[32m+[m[32m  }[m
[32m+[m
[32m+[m[32m  case 'multiparty': {[m
[32m+[m[32m    const { Readable } = require('stream');[m
[32m+[m
[32m+[m[32m    const { Form } = require('multiparty');[m
[32m+[m
[32m+[m[32m    const form = new Form({[m
[32m+[m[32m      maxFieldsSize: Infinity,[m
[32m+[m[32m      maxFields: Infinity,[m
[32m+[m[32m      maxFilesSize: Infinity,[m
[32m+[m[32m      autoFields: false,[m
[32m+[m[32m      autoFiles: false,[m
[32m+[m[32m    });[m
[32m+[m
[32m+[m[32m    const req = new Readable({ read: () => {} });[m
[32m+[m[32m    req.headers = {[m
[32m+[m[32m      'content-type': `multipart/form-data; boundary=${boundary}`,[m
[32m+[m[32m    };[m
[32m+[m
[32m+[m[32m    function hijack(name, fn) {[m
[32m+[m[32m      const oldFn = form[name];[m
[32m+[m[32m      form[name] = function() {[m
[32m+[m[32m        fn();[m
[32m+[m[32m        return oldFn.apply(this, arguments);[m
[32m+[m[32m      };[m
[32m+[m[32m    }[m
[32m+[m
[32m+[m[32m    hijack('onParseHeaderField', () => {[m
[32m+[m[32m      ++calls.headerField;[m
[32m+[m[32m    });[m
[32m+[m[32m    hijack('onParseHeaderValue', () => {[m
[32m+[m[32m      ++calls.headerValue;[m
[32m+[m[32m    });[m
[32m+[m[32m    hijack('onParsePartBegin', () => {[m
[32m+[m[32m      ++calls.partBegin;[m
[32m+[m[32m    });[m
[32m+[m[32m    hijack('onParsePartData', () => {[m
[32m+[m[32m      ++calls.partData;[m
[32m+[m[32m    });[m
[32m+[m[32m    hijack('onParsePartEnd', () => {[m
[32m+[m[32m      ++calls.partEnd;[m
[32m+[m[32m    });[m
[32m+[m
[32m+[m[32m    form.on('close', () => {[m
[32m+[m[32m      ++calls.end;[m
[32m+[m[32m      console.timeEnd(moduleName);[m
[32m+[m[32m    }).on('part', (p) => p.resume());[m
[32m+[m
[32m+[m[32m    console.time(moduleName);[m
[32m+[m[32m    form.parse(req);[m
[32m+[m[32m    for (const buf of buffers)[m
[32m+[m[32m      req.push(buf);[m
[32m+[m[32m    req.push(null);[m
[32m+[m
[32m+[m[32m    break;[m
[32m+[m[32m  }[m
[32m+[m
[32m+[m[32m  default:[m
[32m+[m[32m    if (moduleName === undefined)[m
[32m+[m[32m      console.error('Missing parser module name');[m
[32m+[m[32m    else[m
[32m+[m[32m      console.error(`Invalid parser module name: ${moduleName}`);[m
[32m+[m[32m    process.exit(1);[m
[32m+[m[32m}[m
[1mdiff --git a/node_modules/busboy/bench/bench-urlencoded-fields-100pairs-small.js b/node_modules/busboy/bench/bench-urlencoded-fields-100pairs-small.js[m
[1mnew file mode 100644[m
[1mindex 0000000..5c337df[m
[1m--- /dev/null[m
[1m+++ b/node_modules/busboy/bench/bench-urlencoded-fields-100pairs-small.js[m
[36m@@ -0,0 +1,101 @@[m
[32m+[m[32m'use strict';[m
[32m+[m
[32m+[m[32mconst buffers = [[m
[32m+[m[32m  Buffer.from([m
[32m+[m[32m    (new Array(100)).fill('').map((_, i) => `key${i}=value${i}`).join('&')[m
[32m+[m[32m  ),[m
[32m+[m[32m];[m
[32m+[m[32mconst calls = {[m
[32m+[m[32m  field: 0,[m
[32m+[m[32m  end: 0,[m
[32m+[m[32m};[m
[32m+[m
[32m+[m[32mlet n = 3e3;[m
[32m+[m
[32m+[m[32mconst moduleName = process.argv[2];[m
[32m+[m[32mswitch (moduleName) {[m
[32m+[m[32m  case 'busboy': {[m
[32m+[m[32m    const busboy = require('busboy');[m
[32m+[m
[32m+[m[32m    console.time(moduleName);[m
[32m+[m[32m    (function next() {[m
[32m+[m[32m      const parser = busboy({[m
[32m+[m[32m        limits: {[m
[32m+[m[32m          fieldSizeLimit: Infinity,[m
[32m+[m[32m        },[m
[32m+[m[32m        headers: {[m
[32m+[m[32m          'content-type': 'application/x-www-form-urlencoded; charset=utf-8',[m
[32m+[m[32m        },[m
[32m+[m[32m      });[m
[32m+[m[32m      parser.on('field', (name, val, info) => {[m
[32m+[m[32m        ++calls.field;[m
[32m+[m[32m      }).on('close', () => {[m
[32m+[m[32m        ++calls.end;[m
[32m+[m[32m        if (--n === 0)[m
[32m+[m[32m          console.timeEnd(moduleName);[m
[32m+[m[32m        else[m
[32m+[m[32m          process.nextTick(next);[m
[32m+[m[32m      });[m
[32m+[m
[32m+[m[32m      for (const buf of buffers)[m
[32m+[m[32m        parser.write(buf);[m
[32m+[m[32m      parser.end();[m
[32m+[m[32m    })();[m
[32m+[m[32m    break;[m
[32m+[m[32m  }[m
[32m+[m
[32m+[m[32m  case 'formidable': {[m
[32m+[m[32m    const QuerystringParser =[m
[32m+[m[32m      require('formidable/src/parsers/Querystring.js');[m
[32m+[m
[32m+[m[32m    console.time(moduleName);[m
[32m+[m[32m    (function next() {[m
[32m+[m[32m      const parser = new QuerystringParser();[m
[32m+[m[32m      parser.on('data', (obj) => {[m
[32m+[m[32m        ++calls.field;[m
[32m+[m[32m      }).on('end', () => {[m
[32m+[m[32m        ++calls.end;[m
[32m+[m[32m        if (--n === 0)[m
[32m+[m[32m          console.timeEnd(moduleName);[m
[32m+[m[32m        else[m
[32m+[m[32m          process.nextTick(next);[m
[32m+[m[32m      });[m
[32m+[m
[32m+[m[32m      for (const buf of buffers)[m
[32m+[m[32m        parser.write(buf);[m
[32m+[m[32m      parser.end();[m
[32m+[m[32m    })();[m
[32m+[m[32m    break;[m
[32m+[m[32m  }[m
[32m+[m
[32m+[m[32m  case 'formidable-streaming': {[m
[32m+[m[32m    const QuerystringParser =[m
[32m+[m[32m      require('formidable/src/parsers/StreamingQuerystring.js');[m
[32m+[m
[32m+[m[32m    console.time(moduleName);[m
[32m+[m[32m    (function next() {[m
[32m+[m[32m      const parser = new QuerystringParser();[m
[32m+[m[32m      parser.on('data', (obj) => {[m
[32m+[m[32m        ++calls.field;[m
[32m+[m[32m      }).on('end', () => {[m
[32m+[m[32m        ++calls.end;[m
[32m+[m[32m        if (--n === 0)[m
[32m+[m[32m          console.timeEnd(moduleName);[m
[32m+[m[32m        else[m
[32m+[m[32m          process.nextTick(next);[m
[32m+[m[32m      });[m
[32m+[m
[32m+[m[32m      for (const buf of buffers)[m
[32m+[m[32m        parser.write(buf);[m
[32m+[m[32m      parser.end();[m
[32m+[m[32m    })();[m
[32m+[m[32m    break;[m
[32m+[m[32m  }[m
[32m+[m
[32m+[m[32m  default:[m
[32m+[m[32m    if (moduleName === undefined)[m
[32m+[m[32m      console.error('Missing parser module name');[m
[32m+[m[32m    else[m
[32m+[m[32m      console.error(`Invalid parser module name: ${moduleName}`);[m
[32m+[m[32m    process.exit(1);[m
[32m+[m[32m}[m
[1mdiff --git a/node_modules/busboy/bench/bench-urlencoded-fields-900pairs-small-alt.js b/node_modules/busboy/bench/bench-urlencoded-fields-900pairs-small-alt.js[m
[1mnew file mode 100644[m
[1mindex 0000000..1f5645c[m
[1m--- /dev/null[m
[1m+++ b/node_modules/busboy/bench/bench-urlencoded-fields-900pairs-small-alt.js[m
[36m@@ -0,0 +1,84 @@[m
[32m+[m[32m'use strict';[m
[32m+[m
[32m+[m[32mconst buffers = [[m
[32m+[m[32m  Buffer.from([m
[32m+[m[32m    (new Array(900)).fill('').map((_, i) => `key${i}=value${i}`).join('&')[m
[32m+[m[32m  ),[m
[32m+[m[32m];[m
[32m+[m[32mconst calls = {[m
[32m+[m[32m  field: 0,[m
[32m+[m[32m  end: 0,[m
[32m+[m[32m};[m
[32m+[m
[32m+[m[32mconst moduleName = process.argv[2];[m
[32m+[m[32mswitch (moduleName) {[m
[32m+[m[32m  case 'busboy': {[m
[32m+[m[32m    const busboy = require('busboy');[m
[32m+[m
[32m+[m[32m    console.time(moduleName);[m
[32m+[m[32m    const parser = busboy({[m
[32m+[m[32m      limits: {[m
[32m+[m[32m        fieldSizeLimit: Infinity,[m
[32m+[m[32m      },[m
[32m+[m[32m      headers: {[m
[32m+[m[32m        'content-type': 'application/x-www-form-urlencoded; charset=utf-8',[m
[32m+[m[32m      },[m
[32m+[m[32m    });[m
[32m+[m[32m    parser.on('field', (name, val, info) => {[m
[32m+[m[32m      ++calls.field;[m
[32m+[m[32m    }).on('close', () => {[m
[32m+[m[32m      ++calls.end;[m
[32m+[m[32m      console.timeEnd(moduleName);[m
[32m+[m[32m    });[m
[32m+[m
[32m+[m[32m    for (const buf of buffers)[m
[32m+[m[32m      parser.write(buf);[m
[32m+[m[32m    parser.end();[m
[32m+[m[32m    break;[m
[32m+[m[32m  }[m
[32m+[m
[32m+[m[32m  case 'formidable': {[m
[32m+[m[32m    const QuerystringParser =[m
[32m+[m[32m      require('formidable/src/parsers/Querystring.js');[m
[32m+[m
[32m+[m[32m    console.time(moduleName);[m
[32m+[m[32m    const parser = new QuerystringParser();[m
[32m+[m[32m    parser.on('data', (obj) => {[m
[32m+[m[32m      ++calls.field;[m
[32m+[m[32m    }).on('end', () => {[m
[32m+[m[32m      ++calls.end;[m
[32m+[m[32m      console.timeEnd(moduleName);[m
[32m+[m[32m    });[m
[32m+[m
[32m+[m[32m    for (const buf of buffers)[m
[32m+[m[32m      parser.write(buf);[m
[32m+[m[32m    parser.end();[m
[32m+[m[32m    break;[m
[32m+[m[32m  }[m
[32m+[m
[32m+[m[32m  case 'formidable-streaming': {[m
[32m+[m[32m    const QuerystringParser =[m
[32m+[m[32m      require('formidable/src/parsers/StreamingQuerystring.js');[m
[32m+[m
[32m+[m[32m    console.time(moduleName);[m
[32m+[m[32m    const parser = new QuerystringParser();[m
[32m+[m[32m    parser.on('data', (obj) => {[m
[32m+[m[32m      ++calls.field;[m
[32m+[m[32m    }).on('end', () => {[m
[32m+[m[32m      ++calls.end;[m
[32m+[m[32m      console.timeEnd(moduleName);[m
[32m+[m[32m    });[m
[32m+[m
[32m+[m[32m    for (const buf of buffers)[m
[32m+[m[32m      parser.write(buf);[m
[32m+[m[32m    parser.end();[m
[32m+[m[32m    break;[m
[32m+[m[32m  }[m
[32m+[m
[32m+[m[32m  default:[m
[32m+[m[32m    if (moduleName === undefined)[m
[32m+[m[32m      console.error('Missing parser module name');[m
[32m+[m[32m    else[m
[32m+[m[32m      console.error(`Invalid parser module name: ${moduleName}`);[m
[32m+[m[32m    process.exit(1);[m
[32m+[m[32m}[m
[1mdiff --git a/node_modules/busboy/lib/index.js b/node_modules/busboy/lib/index.js[m
[1mnew file mode 100644[m
[1mindex 0000000..873272d[m
[1m--- /dev/null[m
[1m+++ b/node_modules/busboy/lib/index.js[m
[36m@@ -0,0 +1,57 @@[m
[32m+[m[32m'use strict';[m
[32m+[m
[32m+[m[32mconst { parseContentType } = require('./utils.js');[m
[32m+[m
[32m+[m[32mfunction getInstance(cfg) {[m
[32m+[m[32m  const headers = cfg.headers;[m
[32m+[m[32m  const conType = parseContentType(headers['content-type']);[m
[32m+[m[32m  if (!conType)[m
[32m+[m[32m    throw new Error('Malformed content type');[m
[32m+[m
[32m+[m[32m  for (const type of TYPES) {[m
[32m+[m[32m    const matched = type.detect(conType);[m
[32m+[m[32m    if (!matched)[m
[32m+[m[32m      continue;[m
[32m+[m
[32m+[m[32m    const instanceCfg = {[m
[32m+[m[32m      limits: cfg.limits,[m
[32m+[m[32m      headers,[m
[32m+[m[32m      conType,[m
[32m+[m[32m      highWaterMark: undefined,[m
[32m+[m[32m      fileHwm: undefined,[m
[32m+[m[32m      defCharset: undefined,[m
[32m+[m[32m      defParamCharset: undefined,[m
[32m+[m[32m      preservePath: false,[m
[32m+[m[32m    };[m
[32m+[m[32m    if (cfg.highWaterMark)[m
[32m+[m[32m      instanceCfg.highWaterMark = cfg.highWaterMark;[m
[32m+[m[32m    if (cfg.fileHwm)[m
[32m+[m[32m      instanceCfg.fileHwm = cfg.fileHwm;[m
[32m+[m[32m    instanceCfg.defCharset = cfg.defCharset;[m
[32m+[m[32m    instanceCfg.defParamCharset = cfg.defParamCharset;[m
[32m+[m[32m    instanceCfg.preservePath = cfg.preservePath;[m
[32m+[m[32m    return new type(instanceCfg);[m
[32m+[m[32m  }[m
[32m+[m
[32m+[m[32m  throw new Error(`Unsupported content type: ${headers['content-type']}`);[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32m// Note: types are explicitly listed here for easier bundling[m
[32m+[m[32m// See: https://github.com/mscdex/busboy/issues/121[m
[32m+[m[32mconst TYPES = [[m
[32m+[m[32m  require('./types/multipart'),[m
[32m+[m[32m  require('./types/urlencoded'),[m
[32m+[m[32m].filter(function(typemod) { return typeof typemod.detect === 'function'; });[m
[32m+[m
[32m+[m[32mmodule.exports = (cfg) => {[m
[32m+[m[32m  if (typeof cfg !== 'object' || cfg === null)[m
[32m+[m[32m    cfg = {};[m
[32m+[m
[32m+[m[32m  if (typeof cfg.headers !== 'object'[m
[32m+[m[32m      || cfg.headers === null[m
[32m+[m[32m      || typeof cfg.headers['content-type'] !== 'string') {[m
[32m+[m[32m    throw new Error('Missing Content-Type');[m
[32m+[m[32m  }[m
[32m+[m
[32m+[m[32m  return getInstance(cfg);[m
[32m+[m[32m};[m
[1mdiff --git a/node_modules/busboy/lib/types/multipart.js b/node_modules/busboy/lib/types/multipart.js[m
[1mnew file mode 100644[m
[1mindex 0000000..cc0d7bb[m
[1m--- /dev/null[m
[1m+++ b/node_modules/busboy/lib/types/multipart.js[m
[36m@@ -0,0 +1,653 @@[m
[32m+[m[32m'use strict';[m
[32m+[m
[32m+[m[32mconst { Readable, Writable } = require('stream');[m
[32m+[m
[32m+[m[32mconst StreamSearch = require('streamsearch');[m
[32m+[m
[32m+[m[32mconst {[m
[32m+[m[32m  basename,[m
[32m+[m[32m  convertToUTF8,[m
[32m+[m[32m  getDecoder,[m
[32m+[m[32m  parseContentType,[m
[32m+[m[32m  parseDisposition,[m
[32m+[m[32m} = require('../utils.js');[m
[32m+[m
[32m+[m[32mconst BUF_CRLF = Buffer.from('\r\n');[m
[32m+[m[32mconst BUF_CR = Buffer.from('\r');[m
[32m+[m[32mconst BUF_DASH = Buffer.from('-');[m
[32m+[m
[32m+[m[32mfunction noop() {}[m
[32m+[m
[32m+[m[32mconst MAX_HEADER_PAIRS = 2000; // From node[m
[32m+[m[32mconst MAX_HEADER_SIZE = 16 * 1024; // From node (its default value)[m
[32m+[m
[32m+[m[32mconst HPARSER_NAME = 0;[m
[32m+[m[32mconst HPARSER_PRE_OWS = 1;[m
[32m+[m[32mconst HPARSER_VALUE = 2;[m
[32m+[m[32mclass HeaderParser {[m
[32m+[m[32m  constructor(cb) {[m
[32m+[m[32m    this.header = Object.create(null);[m
[32m+[m[32m    this.pairCount = 0;[m
[32m+[m[32m    this.byteCount = 0;[m
[32m+[m[32m    this.state = HPARSER_NAME;[m
[32m+[m[32m    this.name = '';[m
[32m+[m[32m    this.value = '';[m
[32m+[m[32m    this.crlf = 0;[m
[32m+[m[32m    this.cb = cb;[m
[32m+[m[32m  }[m
[32m+[m
[32m+[m[32m  reset() {[m
[32m+[m[32m    this.header = Object.create(null);[m
[32m+[m[32m    this.pairCount = 0;[m
[32m+[m[32m    this.byteCount = 0;[m
[32m+[m[32m    this.state = HPARSER_NAME;[m
[32m+[m[32m    this.name = '';[m
[32m+[m[32m    this.value = '';[m
[32m+[m[32m    this.crlf = 0;[m
[32m+[m[32m  }[m
[32m+[m
[32m+[m[32m  push(chunk, pos, end) {[m
[32m+[m[32m    let start = pos;[m
[32m+[m[32m    while (pos < end) {[m
[32m+[m[32m      switch (this.state) {[m
[32m+[m[32m        case HPARSER_NAME: {[m
[32m+[m[32m          let done = false;[m
[32m+[m[32m          for (; pos < end; ++pos) {[m
[32m+[m[32m            if (this.byteCount === MAX_HEADER_SIZE)[m
[32m+[m[32m              return -1;[m
[32m+[m[32m            ++this.byteCount;[m
[32m+[m[32m            const code = chunk[pos];[m
[32m+[m[32m            if (TOKEN[code] !== 1) {[m
[32m+[m[32m              if (code !== 58/* ':' */)[m
[32m+[m[32m                return -1;[m
[32m+[m[32m              this.name += chunk.latin1Slice(start, pos);[m
[32m+[m[32m              if (this.name.length === 0)[m
[32m+[m[32m                return -1;[m
[32m+[m[32m              ++pos;[m
[32m+[m[32m              done = true;[m
[32m+[m[32m              this.state = HPARSER_PRE_OWS;[m
[32m+[m[32m              break;[m
[32m+[m[32m            }[m
[32m+[m[32m          }[m
[32m+[m[32m          if (!done) {[m
[32m+[m[32m            this.name += chunk.latin1Slice(start, pos);[m
[32m+[m[32m            break;[m
[32m+[m[32m          }[m
[32m+[m[32m          // FALLTHROUGH[m
[32m+[m[32m        }[m
[32m+[m[32m        case HPARSER_PRE_OWS: {[m
[32m+[m[32m          // Skip optional whitespace[m
[32m+[m[32m          let done = false;[m
[32m+[m[32m          for (; pos < end; ++pos) {[m
[32m+[m[32m            if (this.byteCount === MAX_HEADER_SIZE)[m
[32m+[m[32m              return -1;[m
[32m+[m[32m            ++this.byteCount;[m
[32m+[m[32m            const code = chunk[pos];[m
[32m+[m[32m            if (code !== 32/* ' ' */ && code !== 9/* '\t' */) {[m
[32m+[m[32m              start = pos;[m
[32m+[m[32m              done = true;[m
[32m+[m[32m              this.state = HPARSER_VALUE;[m
[32m+[m[32m              break;[m
[32m+[m[32m            }[m
[32m+[m[32m          }[m
[32m+[m[32m          if (!done)[m
[32m+[m[32m            break;[m
[32m+[m[32m          // FALLTHROUGH[m
[32m+[m[32m        }[m
[32m+[m[32m        case HPARSER_VALUE:[m
[32m+[m[32m          switch (this.crlf) {[m
[32m+[m[32m            case 0: // Nothing yet[m
[32m+[m[32m              for (; pos < end; ++pos) {[m
[32m+[m[32m                if (this.byteCount === MAX_HEADER_SIZE)[m
[32m+[m[32m                  return -1;[m
[32m+[m[32m                ++this.byteCount;[m
[32m+[m[32m                const code = chunk[pos];[m
[32m+[m[32m                if (FIELD_VCHAR[code] !== 1) {[m
[32m+[m[32m                  if (code !== 13/* '\r' */)[m
[32m+[m[32m                    return -1;[m
[32m+[m[32m                  ++this.crlf;[m
[32m+[m[32m                  break;[m
[32m+[m[32m                }[m
[32m+[m[32m              }[m
[32m+[m[32m              this.value += chunk.latin1Slice(start, pos++);[m
[32m+[m[32m              break;[m
[32m+[m[32m            case 1: // Received CR[m
[32m+[m[32m              if (this.byteCount === MAX_HEADER_SIZE)[m
[32m+[m[32m                return -1;[m
[32m+[m[32m              ++this.byteCount;[m
[32m+[m[32m              if (chunk[pos++] !== 10/* '\n' */)[m
[32m+[m[32m                return -1;[m
[32m+[m[32m              ++this.crlf;[m
[32m+[m[32m              break;[m
[32m+[m[32m            case 2: { // Received CR LF[m
[32m+[m[32m              if (this.byteCount === MAX_HEADER_SIZE)[m
[32m+[m[32m                return -1;[m
[32m+[m[32m              ++this.byteCount;[m
[32m+[m[32m              const code = chunk[pos];[m
[32m+[m[32m              if (code === 32/* ' ' */ || code === 9/* '\t' */) {[m
[32m+[m[32m                // Folded value[m
[32m+[m[32m                start = pos;[m
[32m+[m[32m                this.crlf = 0;[m
[32m+[m[32m              } else {[m
[32m+[m[32m                if (++this.pairCount < MAX_HEADER_PAIRS) {[m
[32m+[m[32m                  this.name = this.name.toLowerCase();[m
[32m+[m[32m                  if (this.header[this.name] === undefined)[m
[32m+[m[32m                    this.header[this.name] = [this.value];[m
[32m+[m[32m                  else[m
[32m+[m[32m                    this.header[this.name].push(this.value);[m
[32m+[m[32m                }[m
[32m+[m[32m                if (code === 13/* '\r' */) {[m
[32m+[m[32m                  ++this.crlf;[m
[32m+[m[32m                  ++pos;[m
[32m+[m[32m                } else {[m
[32m+[m[32m                  // Assume start of next header field name[m
[32m+[m[32m                  start = pos;[m
[32m+[m[32m                  this.crlf = 0;[m
[32m+[m[32m                  this.state = HPARSER_NAME;[m
[32m+[m[32m                  this.name = '';[m
[32m+[m[32m                  this.value = '';[m
[32m+[m[32m                }[m
[32m+[m[32m              }[m
[32m+[m[32m              break;[m
[32m+[m[32m            }[m
[32m+[m[32m            case 3: { // Received CR LF CR[m
[32m+[m[32m              if (this.byteCount === MAX_HEADER_SIZE)[m
[32m+[m[32m                return -1;[m
[32m+[m[32m              ++this.byteCount;[m
[32m+[m[32m              if (chunk[pos++] !== 10/* '\n' */)[m
[32m+[m[32m                return -1;[m
[32m+[m[32m              // End of header[m
[32m+[m[32m              const header = this.header;[m
[32m+[m[32m              this.reset();[m
[32m+[m[32m              this.cb(header);[m
[32m+[m[32m              return pos;[m
[32m+[m[32m            }[m
[32m+[m[32m          }[m
[32m+[m[32m          break;[m
[32m+[m[32m      }[m
[32m+[m[32m    }[m
[32m+[m
[32m+[m[32m    return pos;[m
[32m+[m[32m  }[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32mclass FileStream extends Readable {[m
[32m+[m[32m  constructor(opts, owner) {[m
[32m+[m[32m    super(opts);[m
[32m+[m[32m    this.truncated = false;[m
[32m+[m[32m    this._readcb = null;[m
[32m+[m[32m    this.once('end', () => {[m
[32m+[m[32m      // We need to make sure that we call any outstanding _writecb() that is[m
[32m+[m[32m      // associated with this file so that processing of the rest of the form[m
[32m+[m[32m      // can continue. This may not happen if the file stream ends right after[m
[32m+[m[32m      // backpressure kicks in, so we force it here.[m
[32m+[m[32m      this._read();[m
[32m+[m[32m      if (--owner._fileEndsLeft === 0 && owner._finalcb) {[m
[32m+[m[32m        const cb = owner._finalcb;[m
[32m+[m[32m        owner._finalcb = null;[m
[32m+[m[32m        // Make sure other 'end' event handlers get a chance to be executed[m
[32m+[m[32m        // before busboy's 'finish' event is emitted[m
[32m+[m[32m        process.nextTick(cb);[m
[32m+[m[32m      }[m
[32m+[m[32m    });[m
[32m+[m[32m  }[m
[32m+[m[32m  _read(n) {[m
[32m+[m[32m    const cb = this._readcb;[m
[32m+[m[32m    if (cb) {[m
[32m+[m[32m      this._readcb = null;[m
[32m+[m[32m      cb();[m
[32m+[m[32m    }[m
[32m+[m[32m  }[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32mconst ignoreData = {[m
[32m+[m[32m  push: (chunk, pos) => {},[m
[32m+[m[32m  destroy: () => {},[m
[32m+[m[32m};[m
[32m+[m
[32m+[m[32mfunction callAndUnsetCb(self, err) {[m
[32m+[m[32m  const cb = self._writecb;[m
[32m+[m[32m  self._writecb = null;[m
[32m+[m[32m  if (err)[m
[32m+[m[32m    self.destroy(err);[m
[32m+[m[32m  else if (cb)[m
[32m+[m[32m    cb();[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32mfunction nullDecoder(val, hint) {[m
[32m+[m[32m  return val;[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32mclass Multipart extends Writable {[m
[32m+[m[32m  constructor(cfg) {[m
[32m+[m[32m    const streamOpts = {[m
[32m+[m[32m      autoDestroy: true,[m
[32m+[m[32m      emitClose: true,[m
[32m+[m[32m      highWaterMark: (typeof cfg.highWaterMark === 'number'[m
[32m+[m[32m                      ? cfg.highWaterMark[m
[32m+[m[32m                      : undefined),[m
[32m+[m[32m    };[m
[32m+[m[32m    super(streamOpts);[m
[32m+[m
[32m+[m[32m    if (!cfg.conType.params || typeof cfg.conType.params.boundary !== 'string')[m
[32m+[m[32m      throw new Error('Multipart: Boundary not found');[m
[32m+[m
[32m+[m[32m    const boundary = cfg.conType.params.boundary;[m
[32m+[m[32m    const paramDecoder = (typeof cfg.defParamCharset === 'string'[m
[32m+[m[32m                            && cfg.defParamCharset[m
[32m+[m[32m                          ? getDecoder(cfg.defParamCharset)[m
[32m+[m[32m                          : nullDecoder);[m
[32m+[m[32m    const defCharset = (cfg.defCharset || 'utf8');[m
[32m+[m[32m    const preservePath = cfg.preservePath;[m
[32m+[m[32m    const fileOpts = {[m
[32m+[m[32m      autoDestroy: true,[m
[32m+[m[32m      emitClose: true,[m
[32m+[m[32m      highWaterMark: (typeof cfg.fileHwm === 'number'[m
[32m+[m[32m                      ? cfg.fileHwm[m
[32m+[m[32m                      : undefined),[m
[32m+[m[32m    };[m
[32m+[m
[32m+[m[32m    const limits = cfg.limits;[m
[32m+[m[32m    const fieldSizeLimit = (limits && typeof limits.fieldSize === 'number'[m
[32m+[m[32m                            ? limits.fieldSize[m
[32m+[m[32m                            : 1 * 1024 * 1024);[m
[32m+[m[32m    const fileSizeLimit = (limits && typeof limits.fileSize === 'number'[m
[32m+[m[32m                           ? limits.fileSize[m
[32m+[m[32m                           : Infinity);[m
[32m+[m[32m    const filesLimit = (limits && typeof limits.files === 'number'[m
[32m+[m[32m                        ? limits.files[m
[32m+[m[32m                        : Infinity);[m
[32m+[m[32m    const fieldsLimit = (limits && typeof limits.fields === 'number'[m
[32m+[m[32m                         ? limits.fields[m
[32m+[m[32m                         : Infinity);[m
[32m+[m[32m    const partsLimit = (limits && typeof limits.parts === 'number'[m
[32m+[m[32m                        ? limits.parts[m
[32m+[m[32m                        : Infinity);[m
[32m+[m
[32m+[m[32m    let parts = -1; // Account for initial boundary[m
[32m+[m[32m    let fields = 0;[m
[32m+[m[32m    let files = 0;[m
[32m+[m[32m    let skipPart = false;[m
[32m+[m
[32m+[m[32m    this._fileEndsLeft = 0;[m
[32m+[m[32m    this._fileStream = undefined;[m
[32m+[m[32m    this._complete = false;[m
[32m+[m[32m    let fileSize = 0;[m
[32m+[m
[32m+[m[32m    let field;[m
[32m+[m[32m    let fieldSize = 0;[m
[32m+[m[32m    let partCharset;[m
[32m+[m[32m    let partEncoding;[m
[32m+[m[32m    let partType;[m
[32m+[m[32m    let partName;[m
[32m+[m[32m    let partTruncated = false;[m
[32m+[m
[32m+[m[32m    let hitFilesLimit = false;[m
[32m+[m[32m    let hitFieldsLimit = false;[m
[32m+[m
[32m+[m[32m    this._hparser = null;[m
[32m+[m[32m    const hparser = new HeaderParser((header) => {[m
[32m+[m[32m      this._hparser = null;[m
[32m+[m[32m      skipPart = false;[m
[32m+[m
[32m+[m[32m      partType = 'text/plain';[m
[32m+[m[32m      partCharset = defCharset;[m
[32m+[m[32m      partEncoding = '7bit';[m
[32m+[m[32m      partName = undefined;[m
[32m+[m[32m      partTruncated = false;[m
[32m+[m
[32m+[m[32m      let filename;[m
[32m+[m[32m      if (!header['content-disposition']) {[m
[32m+[m[32m        skipPart = true;[m
[32m+[m[32m        return;[m
[32m+[m[32m      }[m
[32m+[m
[32m+[m[32m      const disp = parseDisposition(header['content-disposition'][0],[m
[32m+[m[32m                                    paramDecoder);[m
[32m+[m[32m      if (!disp || disp.type !== 'form-data') {[m
[32m+[m[32m        skipPart = true;[m
[32m+[m[32m        return;[m
[32m+[m[32m      }[m
[32m+[m
[32m+[m[32m      if (disp.params) {[m
[32m+[m[32m        if (disp.params.name)[m
[32m+[m[32m          partName = disp.params.name;[m
[32m+[m
[32m+[m[32m        if (disp.params['filename*'])[m
[32m+[m[32m          filename = disp.params['filename*'];[m
[32m+[m[32m        else if (disp.params.filename)[m
[32m+[m[32m          filename = disp.params.filename;[m
[32m+[m
[32m+[m[32m        if (filename !== undefined && !preservePath)[m
[32m+[m[32m          filename = basename(filename);[m
[32m+[m[32m      }[m
[32m+[m
[32m+[m[32m      if (header['content-type']) {[m
[32m+[m[32m        const conType = parseContentType(header['content-type'][0]);[m
[32m+[m[32m        if (conType) {[m
[32m+[m[32m          partType = `${conType.type}/${conType.subtype}`;[m
[32m+[m[32m          if (conType.params && typeof conType.params.charset === 'string')[m
[32m+[m[32m            partCharset = conType.params.charset.toLowerCase();[m
[32m+[m[32m        }[m
[32m+[m[32m      }[m
[32m+[m
[32m+[m[32m      if (header['content-transfer-encoding'])[m
[32m+[m[32m        partEncoding = header['content-transfer-encoding'][0].toLowerCase();[m
[32m+[m
[32m+[m[32m      if (partType === 'application/octet-stream' || filename !== undefined) {[m
[32m+[m[32m        // File[m
[32m+[m
[32m+[m[32m        if (files === filesLimit) {[m
[32m+[m[32m          if (!hitFilesLimit) {[m
[32m+[m[32m            hitFilesLimit = true;[m
[32m+[m[32m            this.emit('filesLimit');[m
[32m+[m[32m          }[m
[32m+[m[32m          skipPart = true;[m
[32m+[m[32m          return;[m
[32m+[m[32m        }[m
[32m+[m[32m        ++files;[m
[32m+[m
[32m+[m[32m        if (this.listenerCount('file') === 0) {[m
[32m+[m[32m          skipPart = true;[m
[32m+[m[32m          return;[m
[32m+[m[32m        }[m
[32m+[m
[32m+[m[32m        fileSize = 0;[m
[32m+[m[32m        this._fileStream = new FileStream(fileOpts, this);[m
[32m+[m[32m        ++this._fileEndsLeft;[m
[32m+[m[32m        this.emit([m
[32m+[m[32m          'file',[m
[32m+[m[32m          partName,[m
[32m+[m[32m          this._fileStream,[m
[32m+[m[32m          { filename,[m
[32m+[m[32m            encoding: partEncoding,[m
[32m+[m[32m            mimeType: partType }[m
[32m+[m[32m        );[m
[32m+[m[32m      } else {[m
[32m+[m[32m        // Non-file[m
[32m+[m
[32m+[m[32m        if (fields === fieldsLimit) {[m
[32m+[m[32m          if (!hitFieldsLimit) {[m
[32m+[m[32m            hitFieldsLimit = true;[m
[32m+[m[32m            this.emit('fieldsLimit');[m
[32m+[m[32m          }[m
[32m+[m[32m          skipPart = true;[m
[32m+[m[32m          return;[m
[32m+[m[32m        }[m
[32m+[m[32m        ++fields;[m
[32m+[m
[32m+[m[32m        if (this.listenerCount('field') === 0) {[m
[32m+[m[32m          skipPart = true;[m
[32m+[m[32m          return;[m
[32m+[m[32m        }[m
[32m+[m
[32m+[m[32m        field = [];[m
[32m+[m[32m        fieldSize = 0;[m
[32m+[m[32m      }[m
[32m+[m[32m    });[m
[32m+[m
[32m+[m[32m    let matchPostBoundary = 0;[m
[32m+[m[32m    const ssCb = (isMatch, data, start, end, isDataSafe) => {[m
[32m+[m[32mretrydata:[m
[32m+[m[32m      while (data) {[m
[32m+[m[32m        if (this._hparser !== null) {[m
[32m+[m[32m          const ret = this._hparser.push(data, start, end);[m
[32m+[m[32m          if (ret === -1) {[m
[32m+[m[32m            this._hparser = null;[m
[32m+[m[32m            hparser.reset();[m
[32m+[m[32m            this.emit('error', new Error('Malformed part header'));[m
[32m+[m[32m            break;[m
[32m+[m[32m          }[m
[32m+[m[32m          start = ret;[m
[32m+[m[32m        }[m
[32m+[m
[32m+[m[32m        if (start === end)[m
[32m+[m[32m          break;[m
[32m+[m
[32m+[m[32m        if (matchPostBoundary !== 0) {[m
[32m+[m[32m          if (matchPostBoundary === 1) {[m
[32m+[m[32m            switch (data[start]) {[m
[32m+[m[32m              case 45: // '-'[m
[32m+[m[32m                // Try matching '--' after boundary[m
[32m+[m[32m                matchPostBoundary = 2;[m
[32m+[m[32m                ++start;[m
[32m+[m[32m                break;[m
[32m+[m[32m              case 13: // '\r'[m
[32m+[m[32m                // Try matching CR LF before header[m
[32m+[m[32m                matchPostBoundary = 3;[m
[32m+[m[32m                ++start;[m
[32m+[m[32m                break;[m
[32m+[m[32m              default:[m
[32m+[m[32m                matchPostBoundary = 0;[m
[32m+[m[32m            }[m
[32m+[m[32m            if (start === end)[m
[32m+[m[32m              return;[m
[32m+[m[32m          }[m
[32m+[m
[32m+[m[32m          if (matchPostBoundary === 2) {[m
[32m+[m[32m            matchPostBoundary = 0;[m
[32m+[m[32m            if (data[start] === 45/* '-' */) {[m
[32m+[m[32m              // End of multipart data[m
[32m+[m[32m              this._complete = true;[m
[32m+[m[32m              this._bparser = ignoreData;[m
[32m+[m[32m              return;[m
[32m+[m[32m            }[m
[32m+[m[32m            // We saw something other than '-', so put the dash we consumed[m
[32m+[m[32m            // "back"[m
[32m+[m[32m            const writecb = this._writecb;[m
[32m+[m[32m            this._writecb = noop;[m
[32m+[m[32m            ssCb(false, BUF_DASH, 0, 1, false);[m
[32m+[m[32m            this._writecb = writecb;[m
[32m+[m[32m          } else if (matchPostBoundary === 3) {[m
[32m+[m[32m            matchPostBoundary = 0;[m
[32m+[m[32m            if (data[start] === 10/* '\n' */) {[m
[32m+[m[32m              ++start;[m
[32m+[m[32m              if (parts >= partsLimit)[m
[32m+[m[32m                break;[m
[32m+[m[32m              // Prepare the header parser[m
[32m+[m[32m              this._hparser = hparser;[m
[32m+[m[32m              if (start === end)[m
[32m+[m[32m                break;[m
[32m+[m[32m              // Process the remaining data as a header[m
[32m+[m[32m              continue retrydata;[m
[32m+[m[32m            } else {[m
[32m+[m[32m              // We saw something other than LF, so put the CR we consumed[m
[32m+[m[32m              // "back"[m
[32m+[m[32m              const writecb = this._writecb;[m
[32m+[m[32m              this._writecb = noop;[m
[32m+[m[32m              ssCb(false, BUF_CR, 0, 1, false);[m
[32m+[m[32m              this._writecb = writecb;[m
[32m+[m[32m            }[m
[32m+[m[32m          }[m
[32m+[m[32m        }[m
[32m+[m
[32m+[m[32m        if (!skipPart) {[m
[32m+[m[32m          if (this._fileStream) {[m
[32m+[m[32m            let chunk;[m
[32m+[m[32m            const actualLen = Math.min(end - start, fileSizeLimit - fileSize);[m
[32m+[m[32m            if (!isDataSafe) {[m
[32m+[m[32m              chunk = Buffer.allocUnsafe(actualLen);[m
[32m+[m[32m              data.copy(chunk, 0, start, start + actualLen);[m
[32m+[m[32m            } else {[m
[32m+[m[32m              chunk = data.slice(start, start + actualLen);[m
[32m+[m[32m            }[m
[32m+[m
[32m+[m[32m            fileSize += chunk.length;[m
[32m+[m[32m            if (fileSize === fileSizeLimit) {[m
[32m+[m[32m              if (chunk.length > 0)[m
[32m+[m[32m                this._fileStream.push(chunk);[m
[32m+[m[32m              this._fileStream.emit('limit');[m
[32m+[m[32m              this._fileStream.truncated = true;[m
[32m+[m[32m              skipPart = true;[m
[32m+[m[32m            } else if (!this._fileStream.push(chunk)) {[m
[32m+[m[32m              if (this._writecb)[m
[32m+[m[32m                this._fileStream._readcb = this._writecb;[m
[32m+[m[32m              this._writecb = null;[m
[32m+[m[32m            }[m
[32m+[m[32m          } else if (field !== undefined) {[m
[32m+[m[32m            let chunk;[m
[32m+[m[32m            const actualLen = Math.min([m
[32m+[m[32m              end - start,[m
[32m+[m[32m              fieldSizeLimit - fieldSize[m
[32m+[m[32m            );[m
[32m+[m[32m            if (!isDataSafe) {[m
[32m+[m[32m              chunk = Buffer.allocUnsafe(actualLen);[m
[32m+[m[32m              data.copy(chunk, 0, start, start + actualLen);[m
[32m+[m[32m            } else {[m
[32m+[m[32m              chunk = data.slice(start, start + actualLen);[m
[32m+[m[32m            }[m
[32m+[m
[32m+[m[32m            fieldSize += actualLen;[m
[32m+[m[32m            field.push(chunk);[m
[32m+[m[32m            if (fieldSize === fieldSizeLimit) {[m
[32m+[m[32m              skipPart = true;[m
[32m+[m[32m              partTruncated = true;[m
[32m+[m[32m            }[m
[32m+[m[32m          }[m
[32m+[m[32m        }[m
[32m+[m
[32m+[m[32m        break;[m
[32m+[m[32m      }[m
[32m+[m
[32m+[m[32m      if (isMatch) {[m
[32m+[m[32m        matchPostBoundary = 1;[m
[32m+[m
[32m+[m[32m        if (this._fileStream) {[m
[32m+[m[32m          // End the active file stream if the previous part was a file[m
[32m+[m[32m          this._fileStream.push(null);[m
[32m+[m[32m          this._fileStream = null;[m
[32m+[m[32m        } else if (field !== undefined) {[m
[32m+[m[32m          let data;[m
[32m+[m[32m          switch (field.length) {[m
[32m+[m[32m            case 0:[m
[32m+[m[32m              data = '';[m
[32m+[m[32m              break;[m
[32m+[m[32m            case 1:[m
[32m+[m[32m              data = convertToUTF8(field[0], partCharset, 0);[m
[32m+[m[32m              break;[m
[32m+[m[32m            default:[m
[32m+[m[32m              data = convertToUTF8([m
[32m+[m[32m                Buffer.concat(field, fieldSize),[m
[32m+[m[32m                partCharset,[m
[32m+[m[32m                0[m
[32m+[m[32m              );[m
[32m+[m[32m          }[m
[32m+[m[32m          field = undefined;[m
[32m+[m[32m          fieldSize = 0;[m
[32m+[m[32m          this.emit([m
[32m+[m[32m            'field',[m
[32m+[m[32m            partName,[m
[32m+[m[32m            data,[m
[32m+[m[32m            { nameTruncated: false,[m
[32m+[m[32m              valueTruncated: partTruncated,[m
[32m+[m[32m              encoding: partEncoding,[m
[32m+[m[32m              mimeType: partType }[m
[32m+[m[32m          );[m
[32m+[m[32m        }[m
[32m+[m
[32m+[m[32m        if (++parts === partsLimit)[m
[32m+[m[32m          this.emit('partsLimit');[m
[32m+[m[32m      }[m
[32m+[m[32m    };[m
[32m+[m[32m    this._bparser = new StreamSearch(`\r\n--${boundary}`, ssCb);[m
[32m+[m
[32m+[m[32m    this._writecb = null;[m
[32m+[m[32m    this._finalcb = null;[m
[32m+[m
[32m+[m[32m    // Just in case there is no preamble[m
[32m+[m[32m    this.write(BUF_CRLF);[m
[32m+[m[32m  }[m
[32m+[m
[32m+[m[32m  static detect(conType) {[m
[32m+[m[32m    return (conType.type === 'multipart' && conType.subtype === 'form-data');[m
[32m+[m[32m  }[m
[32m+[m
[32m+[m[32m  _write(chunk, enc, cb) {[m
[32m+[m[32m    this._writecb = cb;[m
[32m+[m[32m    this._bparser.push(chunk, 0);[m
[32m+[m[32m    if (this._writecb)[m
[32m+[m[32m      callAndUnsetCb(this);[m
[32m+[m[32m  }[m
[32m+[m
[32m+[m[32m  _destroy(err, cb) {[m
[32m+[m[32m    this._hparser = null;[m
[32m+[m[32m    this._bparser = ignoreData;[m
[32m+[m[32m    if (!err)[m
[32m+[m[32m      err = checkEndState(this);[m
[32m+[m[32m    const fileStream = this._fileStream;[m
[32m+[m[32m    if (fileStream) {[m
[32m+[m[32m      this._fileStream = null;[m
[32m+[m[32m      fileStream.destroy(err);[m
[32m+[m[32m    }[m
[32m+[m[32m    cb(err);[m
[32m+[m[32m  }[m
[32m+[m
[32m+[m[32m  _final(cb) {[m
[32m+[m[32m    this._bparser.destroy();[m
[32m+[m[32m    if (!this._complete)[m
[32m+[m[32m      return cb(new Error('Unexpected end of form'));[m
[32m+[m[32m    if (this._fileEndsLeft)[m
[32m+[m[32m      this._finalcb = finalcb.bind(null, this, cb);[m
[32m+[m[32m    else[m
[32m+[m[32m      finalcb(this, cb);[m
[32m+[m[32m  }[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32mfunction finalcb(self, cb, err) {[m
[32m+[m[32m  if (err)[m
[32m+[m[32m    return cb(err);[m
[32m+[m[32m  err = checkEndState(self);[m
[32m+[m[32m  cb(err);[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32mfunction checkEndState(self) {[m
[32m+[m[32m  if (self._hparser)[m
[32m+[m[32m    return new Error('Malformed part header');[m
[32m+[m[32m  const fileStream = self._fileStream;[m
[32m+[m[32m  if (fileStream) {[m
[32m+[m[32m    self._fileStream = null;[m
[32m+[m[32m    fileStream.destroy(new Error('Unexpected end of file'));[m
[32m+[m[32m  }[m
[32m+[m[32m  if (!self._complete)[m
[32m+[m[32m    return new Error('Unexpected end of form');[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32mconst TOKEN = [[m
[32m+[m[32m  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,[m
[32m+[m[32m  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,[m
[32m+[m[32m  0, 1, 0, 1, 1, 1, 1, 1, 0, 0, 1, 1, 0, 1, 1, 0,[m
[32m+[m[32m  1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0,[m
[32m+[m[32m  0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,[m
[32m+[m[32m  1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1,[m
[32m+[m[32m  1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,[m
[32m+[m[32m  1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0, 1, 0,[m
[32m+[m[32m  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,[m
[32m+[m[32m  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,[m
[32m+[m[32m  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,[m
[32m+[m[32m  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,[m
[32m+[m[32m  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,[m
[32m+[m[32m  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,[m
[32m+[m[32m  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,[m
[32m+[m[32m  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,[m
[32m+[m[32m];[m
[32m+[m
[32m+[m[32mconst FIELD_VCHAR = [[m
[32m+[m[32m  0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0,[m
[32m+[m[32m  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,[m
[32m+[m[32m  1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,[m
[32m+[m[32m  1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,[m
[32m+[m[32m  1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,[m
[32m+[m[32m  1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,[m
[32m+[m[32m  1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,[m
[32m+[m[32m  1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0,[m
[32m+[m[32m  1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,[m
[32m+[m[32m  1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,[m
[32m+[m[32m  1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,[m
[32m+[m[32m  1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,[m
[32m+[m[32m  1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,[m
[32m+[m[32m  1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,[m
[32m+[m[32m  1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,[m
[32m+[m[32m  1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,[m
[32m+[m[32m];[m
[32m+[m
[32m+[m[32mmodule.exports = Multipart;[m
[1mdiff --git a/node_modules/busboy/lib/types/urlencoded.js b/node_modules/busboy/lib/types/urlencoded.js[m
[1mnew file mode 100644[m
[1mindex 0000000..5c463a2[m
[1m--- /dev/null[m
[1m+++ b/node_modules/busboy/lib/types/urlencoded.js[m
[36m@@ -0,0 +1,350 @@[m
[32m+[m[32m'use strict';[m
[32m+[m
[32m+[m[32mconst { Writable } = require('stream');[m
[32m+[m
[32m+[m[32mconst { getDecoder } = require('../utils.js');[m
[32m+[m
[32m+[m[32mclass URLEncoded extends Writable {[m
[32m+[m[32m  constructor(cfg) {[m
[32m+[m[32m    const streamOpts = {[m
[32m+[m[32m      autoDestroy: true,[m
[32m+[m[32m      emitClose: true,[m
[32m+[m[32m      highWaterMark: (typeof cfg.highWaterMark === 'number'[m
[32m+[m[32m                      ? cfg.highWaterMark[m
[32m+[m[32m                      : undefined),[m
[32m+[m[32m    };[m
[32m+[m[32m    super(streamOpts);[m
[32m+[m
[32m+[m[32m    let charset = (cfg.defCharset || 'utf8');[m
[32m+[m[32m    if (cfg.conType.params && typeof cfg.conType.params.charset === 'string')[m
[32m+[m[32m      charset = cfg.conType.params.charset;[m
[32m+[m
[32m+[m[32m    this.charset = charset;[m
[32m+[m
[32m+[m[32m    const limits = cfg.limits;[m
[32m+[m[32m    this.fieldSizeLimit = (limits && typeof limits.fieldSize === 'number'[m
[32m+[m[32m                           ? limits.fieldSize[m
[32m+[m[32m                           : 1 * 1024 * 1024);[m
[32m+[m[32m    this.fieldsLimit = (limits && typeof limits.fields === 'number'[m
[32m+[m[32m                        ? limits.fields[m
[32m+[m[32m                        : Infinity);[m
[32m+[m[32m    this.fieldNameSizeLimit = ([m
[32m+[m[32m      limits && typeof limits.fieldNameSize === 'number'[m
[32m+[m[32m      ? limits.fieldNameSize[m
[32m+[m[32m      : 100[m
[32m+[m[32m    );[m
[32m+[m
[32m+[m[32m    this._inKey = true;[m
[32m+[m[32m    this._keyTrunc = false;[m
[32m+[m[32m    this._valTrunc = false;[m
[32m+[m[32m    this._bytesKey = 0;[m
[32m+[m[32m    this._bytesVal = 0;[m
[32m+[m[32m    this._fields = 0;[m
[32m+[m[32m    this._key = '';[m
[32m+[m[32m    this._val = '';[m
[32m+[m[32m    this._byte = -2;[m
[32m+[m[32m    this._lastPos = 0;[m
[32m+[m[32m    this._encode = 0;[m
[32m+[m[32m    this._decoder = getDecoder(charset);[m
[32m+[m[32m  }[m
[32m+[m
[32m+[m[32m  static detect(conType) {[m
[32m+[m[32m    return (conType.type === 'application'[m
[32m+[m[32m            && conType.subtype === 'x-www-form-urlencoded');[m
[32m+[m[32m  }[m
[32m+[m
[32m+[m[32m  _write(chunk, enc, cb) {[m
[32m+[m[32m    if (this._fields >= this.fieldsLimit)[m
[32m+[m[32m      return cb();[m
[32m+[m
[32m+[m[32m    let i = 0;[m
[32m+[m[32m    const len = chunk.length;[m
[32m+[m[32m    this._lastPos = 0;[m
[32m+[m
[32m+[m[32m    // Check if we last ended mid-percent-encoded byte[m
[32m+[m[32m    if (this._byte !== -2) {[m
[32m+[m[32m      i = readPctEnc(this, chunk, i, len);[m
[32m+[m[32m      if (i === -1)[m
[32m+[m[32m        return cb(new Error('Malformed urlencoded form'));[m
[32m+[m[32m      if (i >= len)[m
[32m+[m[32m        return cb();[m
[32m+[m[32m      if (this._inKey)[m
[32m+[m[32m        ++this._bytesKey;[m
[32m+[m[32m      else[m
[32m+[m[32m        ++this._bytesVal;[m
[32m+[m[32m    }[m
[32m+[m
[32m+[m[32mmain:[m
[32m+[m[32m    while (i < len) {[m
[32m+[m[32m      if (this._inKey) {[m
[32m+[m[32m        // Parsing key[m
[32m+[m
[32m+[m[32m        i = skipKeyBytes(this, chunk, i, len);[m
[32m+[m
[32m+[m[32m        while (i < len) {[m
[32m+[m[32m          switch (chunk[i]) {[m
[32m+[m[32m            case 61: // '='[m
[32m+[m[32m              if (this._lastPos < i)[m
[32m+[m[32m                this._key += chunk.latin1Slice(this._lastPos, i);[m
[32m+[m[32m              this._lastPos = ++i;[m
[32m+[m[32m              this._key = this._decoder(this._key, this._encode);[m
[32m+[m[32m              this._encode = 0;[m
[32m+[m[32m              this._inKey = false;[m
[32m+[m[32m              continue main;[m
[32m+[m[32m            case 38: // '&'[m
[32m+[m[32m              if (this._lastPos < i)[m
[32m+[m[32m                this._key += chunk.latin1Slice(this._lastPos, i);[m
[32m+[m[32m              this._lastPos = ++i;[m
[32m+[m[32m              this._key = this._decoder(this._key, this._encode);[m
[32m+[m[32m              this._encode = 0;[m
[32m+[m[32m              if (this._bytesKey > 0) {[m
[32m+[m[32m                this.emit([m
[32m+[m[32m                  'field',[m
[32m+[m[32m                  this._key,[m
[32m+[m[32m                  '',[m
[32m+[m[32m                  { nameTruncated: this._keyTrunc,[m
[32m+[m[32m                    valueTruncated: false,[m
[32m+[m[32m                    encoding: this.charset,[m
[32m+[m[32m                    mimeType: 'text/plain' }[m
[32m+[m[32m                );[m
[32m+[m[32m              }[m
[32m+[m[32m              this._key = '';[m
[32m+[m[32m              this._val = '';[m
[32m+[m[32m              this._keyTrunc = false;[m
[32m+[m[32m              this._valTrunc = false;[m
[32m+[m[32m              this._bytesKey = 0;[m
[32m+[m[32m              this._bytesVal = 0;[m
[32m+[m[32m              if (++this._fields >= this.fieldsLimit) {[m
[32m+[m[32m                this.emit('fieldsLimit');[m
[32m+[m[32m                return cb();[m
[32m+[m[32m              }[m
[32m+[m[32m              continue;[m
[32m+[m[32m            case 43: // '+'[m
[32m+[m[32m              if (this._lastPos < i)[m
[32m+[m[32m                this._key += chunk.latin1Slice(this._lastPos, i);[m
[32m+[m[32m              this._key += ' ';[m
[32m+[m[32m              this._lastPos = i + 1;[m
[32m+[m[32m              break;[m
[32m+[m[32m            case 37: // '%'[m
[32m+[m[32m              if (this._encode === 0)[m
[32m+[m[32m                this._encode = 1;[m
[32m+[m[32m              if (this._lastPos < i)[m
[32m+[m[32m                this._key += chunk.latin1Slice(this._lastPos, i);[m
[32m+[m[32m              this._lastPos = i + 1;[m
[32m+[m[32m              this._byte = -1;[m
[32m+[m[32m              i = readPctEnc(this, chunk, i + 1, len);[m
[32m+[m[32m              if (i === -1)[m
[32m+[m[32m                return cb(new Error('Malformed urlencoded form'));[m
[32m+[m[32m              if (i >= len)[m
[32m+[m[32m                return cb();[m
[32m+[m[32m              ++this._bytesKey;[m
[32m+[m[32m              i = skipKeyBytes(this, chunk, i, len);[m
[32m+[m[32m              continue;[m
[32m+[m[32m          }[m
[32m+[m[32m          ++i;[m
[32m+[m[32m          ++this._bytesKey;[m
[32m+[m[32m          i = skipKeyBytes(this, chunk, i, len);[m
[32m+[m[32m        }[m
[32m+[m[32m        if (this._lastPos < i)[m
[32m+[m[32m          this._key += chunk.latin1Slice(this._lastPos, i);[m
[32m+[m[32m      } else {[m
[32m+[m[32m        // Parsing value[m
[32m+[m
[32m+[m[32m        i = skipValBytes(this, chunk, i, len);[m
[32m+[m
[32m+[m[32m        while (i < len) {[m
[32m+[m[32m          switch (chunk[i]) {[m
[32m+[m[32m            case 38: // '&'[m
[32m+[m[32m              if (this._lastPos < i)[m
[32m+[m[32m                this._val += chunk.latin1Slice(this._lastPos, i);[m
[32m+[m[32m              this._lastPos = ++i;[m
[32m+[m[32m              this._inKey = true;[m
[32m+[m[32m              this._val = this._decoder(this._val, this._encode);[m
[32m+[m[32m              this._encode = 0;[m
[32m+[m[32m              if (this._bytesKey > 0 || this._bytesVal > 0) {[m
[32m+[m[32m                this.emit([m
[32m+[m[32m                  'field',[m
[32m+[m[32m                  this._key,[m
[32m+[m[32m                  this._val,[m
[32m+[m[32m                  { nameTruncated: this._keyTrunc,[m
[32m+[m[32m                    valueTruncated: this._valTrunc,[m
[32m+[m[32m                    encoding: this.charset,[m
[32m+[m[32m                    mimeType: 'text/plain' }[m
[32m+[m[32m                );[m
[32m+[m[32m              }[m
[32m+[m[32m              this._key = '';[m
[32m+[m[32m              this._val = '';[m
[32m+[m[32m              this._keyTrunc = false;[m
[32m+[m[32m              this._valTrunc = false;[m
[32m+[m[32m              this._bytesKey = 0;[m
[32m+[m[32m              this._bytesVal = 0;[m
[32m+[m[32m              if (++this._fields >= this.fieldsLimit) {[m
[32m+[m[32m                this.emit('fieldsLimit');[m
[32m+[m[32m                return cb();[m
[32m+[m[32m              }[m
[32m+[m[32m              continue main;[m
[32m+[m[32m            case 43: // '+'[m
[32m+[m[32m              if (this._lastPos < i)[m
[32m+[m[32m                this._val += chunk.latin1Slice(this._lastPos, i);[m
[32m+[m[32m              this._val += ' ';[m
[32m+[m[32m              this._lastPos = i + 1;[m
[32m+[m[32m              break;[m
[32m+[m[32m            case 37: // '%'[m
[32m+[m[32m              if (this._encode === 0)[m
[32m+[m[32m                this._encode = 1;[m
[32m+[m[32m              if (this._lastPos < i)[m
[32m+[m[32m                this._val += chunk.latin1Slice(this._lastPos, i);[m
[32m+[m[32m              this._lastPos = i + 1;[m
[32m+[m[32m              this._byte = -1;[m
[32m+[m[32m              i = readPctEnc(this, chunk, i + 1, len);[m
[32m+[m[32m              if (i === -1)[m
[32m+[m[32m                return cb(new Error('Malformed urlencoded form'));[m
[32m+[m[32m              if (i >= len)[m
[32m+[m[32m                return cb();[m
[32m+[m[32m              ++this._bytesVal;[m
[32m+[m[32m              i = skipValBytes(this, chunk, i, len);[m
[32m+[m[32m              continue;[m
[32m+[m[32m          }[m
[32m+[m[32m          ++i;[m
[32m+[m[32m          ++this._bytesVal;[m
[32m+[m[32m          i = skipValBytes(this, chunk, i, len);[m
[32m+[m[32m        }[m
[32m+[m[32m        if (this._lastPos < i)[m
[32m+[m[32m          this._val += chunk.latin1Slice(this._lastPos, i);[m
[32m+[m[32m      }[m
[32m+[m[32m    }[m
[32m+[m
[32m+[m[32m    cb();[m
[32m+[m[32m  }[m
[32m+[m
[32m+[m[32m  _final(cb) {[m
[32m+[m[32m    if (this._byte !== -2)[m
[32m+[m[32m      return cb(new Error('Malformed urlencoded form'));[m
[32m+[m[32m    if (!this._inKey || this._bytesKey > 0 || this._bytesVal > 0) {[m
[32m+[m[32m      if (this._inKey)[m
[32m+[m[32m        this._key = this._decoder(this._key, this._encode);[m
[32m+[m[32m      else[m
[32m+[m[32m        this._val = this._decoder(this._val, this._encode);[m
[32m+[m[32m      this.emit([m
[32m+[m[32m        'field',[m
[32m+[m[32m        this._key,[m
[32m+[m[32m        this._val,[m
[32m+[m[32m        { nameTruncated: this._keyTrunc,[m
[32m+[m[32m          valueTruncated: this._valTrunc,[m
[32m+[m[32m          encoding: this.charset,[m
[32m+[m[32m          mimeType: 'text/plain' }[m
[32m+[m[32m      );[m
[32m+[m[32m    }[m
[32m+[m[32m    cb();[m
[32m+[m[32m  }[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32mfunction readPctEnc(self, chunk, pos, len) {[m
[32m+[m[32m  if (pos >= len)[m
[32m+[m[32m    return len;[m
[32m+[m
[32m+[m[32m  if (self._byte === -1) {[m
[32m+[m[32m    // We saw a '%' but no hex characters yet[m
[32m+[m[32m    const hexUpper = HEX_VALUES[chunk[pos++]];[m
[32m+[m[32m    if (hexUpper === -1)[m
[32m+[m[32m      return -1;[m
[32m+[m
[32m+[m[32m    if (hexUpper >= 8)[m
[32m+[m[32m      self._encode = 2; // Indicate high bits detected[m
[32m+[m
[32m+[m[32m    if (pos < len) {[m
[32m+[m[32m      // Both hex characters are in this chunk[m
[32m+[m[32m      const hexLower = HEX_VALUES[chunk[pos++]];[m
[32m+[m[32m      if (hexLower === -1)[m
[32m+[m[32m        return -1;[m
[32m+[m
[32m+[m[32m      if (self._inKey)[m
[32m+[m[32m        self._key += String.fromCharCode((hexUpper << 4) + hexLower);[m
[32m+[m[32m      else[m
[32m+[m[32m        self._val += String.fromCharCode((hexUpper << 4) + hexLower);[m
[32m+[m
[32m+[m[32m      self._byte = -2;[m
[32m+[m[32m      self._lastPos = pos;[m
[32m+[m[32m    } else {[m
[32m+[m[32m      // Only one hex character was available in this chunk[m
[32m+[m[32m      self._byte = hexUpper;[m
[32m+[m[32m    }[m
[32m+[m[32m  } else {[m
[32m+[m[32m    // We saw only one hex character so far[m
[32m+[m[32m    const hexLower = HEX_VALUES[chunk[pos++]];[m
[32m+[m[32m    if (hexLower === -1)[m
[32m+[m[32m      return -1;[m
[32m+[m
[32m+[m[32m    if (self._inKey)[m
[32m+[m[32m      self._key += String.fromCharCode((self._byte << 4) + hexLower);[m
[32m+[m[32m    else[m
[32m+[m[32m      self._val += String.fromCharCode((self._byte << 4) + hexLower);[m
[32m+[m
[32m+[m[32m    self._byte = -2;[m
[32m+[m[32m    self._lastPos = pos;[m
[32m+[m[32m  }[m
[32m+[m
[32m+[m[32m  return pos;[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32mfunction skipKeyBytes(self, chunk, pos, len) {[m
[32m+[m[32m  // Skip bytes if we've truncated[m
[32m+[m[32m  if (self._bytesKey > self.fieldNameSizeLimit) {[m
[32m+[m[32m    if (!self._keyTrunc) {[m
[32m+[m[32m      if (self._lastPos < pos)[m
[32m+[m[32m        self._key += chunk.latin1Slice(self._lastPos, pos - 1);[m
[32m+[m[32m    }[m
[32m+[m[32m    self._keyTrunc = true;[m
[32m+[m[32m    for (; pos < len; ++pos) {[m
[32m+[m[32m      const code = chunk[pos];[m
[32m+[m[32m      if (code === 61/* '=' */ || code === 38/* '&' */)[m
[32m+[m[32m        break;[m
[32m+[m[32m      ++self._bytesKey;[m
[32m+[m[32m    }[m
[32m+[m[32m    self._lastPos = pos;[m
[32m+[m[32m  }[m
[32m+[m
[32m+[m[32m  return pos;[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32mfunction skipValBytes(self, chunk, pos, len) {[m
[32m+[m[32m  // Skip bytes if we've truncated[m
[32m+[m[32m  if (self._bytesVal > self.fieldSizeLimit) {[m
[32m+[m[32m    if (!self._valTrunc) {[m
[32m+[m[32m      if (self._lastPos < pos)[m
[32m+[m[32m        self._val += chunk.latin1Slice(self._lastPos, pos - 1);[m
[32m+[m[32m    }[m
[32m+[m[32m    self._valTrunc = true;[m
[32m+[m[32m    for (; pos < len; ++pos) {[m
[32m+[m[32m      if (chunk[pos] === 38/* '&' */)[m
[32m+[m[32m        break;[m
[32m+[m[32m      ++self._bytesVal;[m
[32m+[m[32m    }[m
[32m+[m[32m    self._lastPos = pos;[m
[32m+[m[32m  }[m
[32m+[m
[32m+[m[32m  return pos;[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32m/* eslint-disable no-multi-spaces */[m
[32m+[m[32mconst HEX_VALUES = [[m
[32m+[m[32m  -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,[m
[32m+[m[32m  -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,[m
[32m+[m[32m  -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,[m
[32m+[m[32m   0,  1,  2,  3,  4,  5,  6,  7,  8,  9, -1, -1, -1, -1, -1, -1,[m
[32m+[m[32m  -1, 10, 11, 12, 13, 14, 15, -1, -1, -1, -1, -1, -1, -1, -1, -1,[m
[32m+[m[32m  -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,[m
[32m+[m[32m  -1, 10, 11, 12, 13, 14, 15, -1, -1, -1, -1, -1, -1, -1, -1, -1,[m
[32m+[m[32m  -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,[m
[32m+[m[32m  -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,[m
[32m+[m[32m  -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,[m
[32m+[m[32m  -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,[m
[32m+[m[32m  -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,[m
[32m+[m[32m  -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,[m
[32m+[m[32m  -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,[m
[32m+[m[32m  -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,[m
[32m+[m[32m  -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,[m
[32m+[m[32m];[m
[32m+[m[32m/* eslint-enable no-multi-spaces */[m
[32m+[m
[32m+[m[32mmodule.exports = URLEncoded;[m
[1mdiff --git a/node_modules/busboy/lib/utils.js b/node_modules/busboy/lib/utils.js[m
[1mnew file mode 100644[m
[1mindex 0000000..8274f6c[m
[1m--- /dev/null[m
[1m+++ b/node_modules/busboy/lib/utils.js[m
[36m@@ -0,0 +1,596 @@[m
[32m+[m[32m'use strict';[m
[32m+[m
[32m+[m[32mfunction parseContentType(str) {[m
[32m+[m[32m  if (str.length === 0)[m
[32m+[m[32m    return;[m
[32m+[m
[32m+[m[32m  const params = Object.create(null);[m
[32m+[m[32m  let i = 0;[m
[32m+[m
[32m+[m[32m  // Parse type[m
[32m+[m[32m  for (; i < str.length; ++i) {[m
[32m+[m[32m    const code = str.charCodeAt(i);[m
[32m+[m[32m    if (TOKEN[code] !== 1) {[m
[32m+[m[32m      if (code !== 47/* '/' */ || i === 0)[m
[32m+[m[32m        return;[m
[32m+[m[32m      break;[m
[32m+[m[32m    }[m
[32m+[m[32m  }[m
[32m+[m[32m  // Check for type without subtype[m
[32m+[m[32m  if (i === str.length)[m
[32m+[m[32m    return;[m
[32m+[m
[32m+[m[32m  const type = str.slice(0, i).toLowerCase();[m
[32m+[m
[32m+[m[32m  // Parse subtype[m
[32m+[m[32m  const subtypeStart = ++i;[m
[32m+[m[32m  for (; i < str.length; ++i) {[m
[32m+[m[32m    const code = str.charCodeAt(i);[m
[32m+[m[32m    if (TOKEN[code] !== 1) {[m
[32m+[m[32m      // Make sure we have a subtype[m
[32m+[m[32m      if (i === subtypeStart)[m
[32m+[m[32m        return;[m
[32m+[m
[32m+[m[32m      if (parseContentTypeParams(str, i, params) === undefined)[m
[32m+[m[32m        return;[m
[32m+[m[32m      break;[m
[32m+[m[32m    }[m
[32m+[m[32m  }[m
[32m+[m[32m  // Make sure we have a subtype[m
[32m+[m[32m  if (i === subtypeStart)[m
[32m+[m[32m    return;[m
[32m+[m
[32m+[m[32m  const subtype = str.slice(subtypeStart, i).toLowerCase();[m
[32m+[m
[32m+[m[32m  return { type, subtype, params };[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32mfunction parseContentTypeParams(str, i, params) {[m
[32m+[m[32m  while (i < str.length) {[m
[32m+[m[32m    // Consume whitespace[m
[32m+[m[32m    for (; i < str.length; ++i) {[m
[32m+[m[32m      const code = str.charCodeAt(i);[m
[32m+[m[32m      if (code !== 32/* ' ' */ && code !== 9/* '\t' */)[m
[32m+[m[32m        break;[m
[32m+[m[32m    }[m
[32m+[m
[32m+[m[32m    // Ended on whitespace[m
[32m+[m[32m    if (i === str.length)[m
[32m+[m[32m      break;[m
[32m+[m
[32m+[m[32m    // Check for malformed parameter[m
[32m+[m[32m    if (str.charCodeAt(i++) !== 59/* ';' */)[m
[32m+[m[32m      return;[m
[32m+[m
[32m+[m[32m    // Consume whitespace[m
[32m+[m[32m    for (; i < str.length; ++i) {[m
[32m+[m[32m      const code = str.charCodeAt(i);[m
[32m+[m[32m      if (code !== 32/* ' ' */ && code !== 9/* '\t' */)[m
[32m+[m[32m        break;[m
[32m+[m[32m    }[m
[32m+[m
[32m+[m[32m    // Ended on whitespace (malformed)[m
[32m+[m[32m    if (i === str.length)[m
[32m+[m[32m      return;[m
[32m+[m
[32m+[m[32m    let name;[m
[32m+[m[32m    const nameStart = i;[m
[32m+[m[32m    // Parse parameter name[m
[32m+[m[32m    for (; i < str.length; ++i) {[m
[32m+[m[32m      const code = str.charCodeAt(i);[m
[32m+[m[32m      if (TOKEN[code] !== 1) {[m
[32m+[m[32m        if (code !== 61/* '=' */)[m
[32m+[m[32m          return;[m
[32m+[m[32m        break;[m
[32m+[m[32m      }[m
[32m+[m[32m    }[m
[32m+[m
[32m+[m[32m    // No value (malformed)[m
[32m+[m[32m    if (i === str.length)[m
[32m+[m[32m      return;[m
[32m+[m
[32m+[m[32m    name = str.slice(nameStart, i);[m
[32m+[m[32m    ++i; // Skip over '='[m
[32m+[m
[32m+[m[32m    // No value (malformed)[m
[32m+[m[32m    if (i === str.length)[m
[32m+[m[32m      return;[m
[32m+[m
[32m+[m[32m    let value = '';[m
[32m+[m[32m    let valueStart;[m
[32m+[m[32m    if (str.charCodeAt(i) === 34/* '"' */) {[m
[32m+[m[32m      valueStart = ++i;[m
[32m+[m[32m      let escaping = false;[m
[32m+[m[32m      // Parse quoted value[m
[32m+[m[32m      for (; i < str.length; ++i) {[m
[32m+[m[32m        const code = str.charCodeAt(i);[m
[32m+[m[32m        if (code === 92/* '\\' */) {[m
[32m+[m[32m          if (escaping) {[m
[32m+[m[32m            valueStart = i;[m
[32m+[m[32m            escaping = false;[m
[32m+[m[32m          } else {[m
[32m+[m[32m            value += str.slice(valueStart, i);[m
[32m+[m[32m            escaping = true;[m
[32m+[m[32m          }[m
[32m+[m[32m          continue;[m
[32m+[m[32m        }[m
[32m+[m[32m        if (code === 34/* '"' */) {[m
[32m+[m[32m          if (escaping) {[m
[32m+[m[32m            valueStart = i;[m
[32m+[m[32m            escaping = false;[m
[32m+[m[32m            continue;[m
[32m+[m[32m          }[m
[32m+[m[32m          value += str.slice(valueStart, i);[m
[32m+[m[32m          break;[m
[32m+[m[32m        }[m
[32m+[m[32m        if (escaping) {[m
[32m+[m[32m          valueStart = i - 1;[m
[32m+[m[32m          escaping = false;[m
[32m+[m[32m        }[m
[32m+[m[32m        // Invalid unescaped quoted character (malformed)[m
[32m+[m[32m        if (QDTEXT[code] !== 1)[m
[32m+[m[32m          return;[m
[32m+[m[32m      }[m
[32m+[m
[32m+[m[32m      // No end quote (malformed)[m
[32m+[m[32m      if (i === str.length)[m
[32m+[m[32m        return;[m
[32m+[m
[32m+[m[32m      ++i; // Skip over double quote[m
[32m+[m[32m    } else {[m
[32m+[m[32m      valueStart = i;[m
[32m+[m[32m      // Parse unquoted value[m
[32m+[m[32m      for (; i < str.length; ++i) {[m
[32m+[m[32m        const code = str.charCodeAt(i);[m
[32m+[m[32m        if (TOKEN[code] !== 1) {[m
[32m+[m[32m          // No value (malformed)[m
[32m+[m[32m          if (i === valueStart)[m
[32m+[m[32m            return;[m
[32m+[m[32m          break;[m
[32m+[m[32m        }[m
[32m+[m[32m      }[m
[32m+[m[32m      value = str.slice(valueStart, i);[m
[32m+[m[32m    }[m
[32m+[m
[32m+[m[32m    name = name.toLowerCase();[m
[32m+[m[32m    if (params[name] === undefined)[m
[32m+[m[32m      params[name] = value;[m
[32m+[m[32m  }[m
[32m+[m
[32m+[m[32m  return params;[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32mfunction parseDisposition(str, defDecoder) {[m
[32m+[m[32m  if (str.length === 0)[m
[32m+[m[32m    return;[m
[32m+[m
[32m+[m[32m  const params = Object.create(null);[m
[32m+[m[32m  let i = 0;[m
[32m+[m
[32m+[m[32m  for (; i < str.length; ++i) {[m
[32m+[m[32m    const code = str.charCodeAt(i);[m
[32m+[m[32m    if (TOKEN[code] !== 1) {[m
[32m+[m[32m      if (parseDispositionParams(str, i, params, defDecoder) === undefined)[m
[32m+[m[32m        return;[m
[32m+[m[32m      break;[m
[32m+[m[32m    }[m
[32m+[m[32m  }[m
[32m+[m
[32m+[m[32m  const type = str.slice(0, i).toLowerCase();[m
[32m+[m
[32m+[m[32m  return { type, params };[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32mfunction parseDispositionParams(str, i, params, defDecoder) {[m
[32m+[m[32m  while (i < str.length) {[m
[32m+[m[32m    // Consume whitespace[m
[32m+[m[32m    for (; i < str.length; ++i) {[m
[32m+[m[32m      const code = str.charCodeAt(i);[m
[32m+[m[32m      if (code !== 32/* ' ' */ && code !== 9/* '\t' */)[m
[32m+[m[32m        break;[m
[32m+[m[32m    }[m
[32m+[m
[32m+[m[32m    // Ended on whitespace[m
[32m+[m[32m    if (i === str.length)[m
[32m+[m[32m      break;[m
[32m+[m
[32m+[m[32m    // Check for malformed parameter[m
[32m+[m[32m    if (str.charCodeAt(i++) !== 59/* ';' */)[m
[32m+[m[32m      return;[m
[32m+[m
[32m+[m[32m    // Consume whitespace[m
[32m+[m[32m    for (; i < str.length; ++i) {[m
[32m+[m[32m      const code = str.charCodeAt(i);[m
[32m+[m[32m      if (code !== 32/* ' ' */ && code !== 9/* '\t' */)[m
[32m+[m[32m        break;[m
[32m+[m[32m    }[m
[32m+[m
[32m+[m[32m    // Ended on whitespace (malformed)[m
[32m+[m[32m    if (i === str.length)[m
[32m+[m[32m      return;[m
[32m+[m
[32m+[m[32m    let name;[m
[32m+[m[32m    const nameStart = i;[m
[32m+[m[32m    // Parse parameter name[m
[32m+[m[32m    for (; i < str.length; ++i) {[m
[32m+[m[32m      const code = str.charCodeAt(i);[m
[32m+[m[32m      if (TOKEN[code] !== 1) {[m
[32m+[m[32m        if (code === 61/* '=' */)[m
[32m+[m[32m          break;[m
[32m+[m[32m        return;[m
[32m+[m[32m      }[m
[32m+[m[32m    }[m
[32m+[m
[32m+[m[32m    // No value (malformed)[m
[32m+[m[32m    if (i === str.length)[m
[32m+[m[32m      return;[m
[32m+[m
[32m+[m[32m    let value = '';[m
[32m+[m[32m    let valueStart;[m
[32m+[m[32m    let charset;[m
[32m+[m[32m    //~ let lang;[m
[32m+[m[32m    name = str.slice(nameStart, i);[m
[32m+[m[32m    if (name.charCodeAt(name.length - 1) === 42/* '*' */) {[m
[32m+[m[32m      // Extended value[m
[32m+[m
[32m+[m[32m      const charsetStart = ++i;[m
[32m+[m[32m      // Parse charset name[m
[32m+[m[32m      for (; i < str.length; ++i) {[m
[32m+[m[32m        const code = str.charCodeAt(i);[m
[32m+[m[32m        if (CHARSET[code] !== 1) {[m
[32m+[m[32m          if (code !== 39/* '\'' */)[m
[32m+[m[32m            return;[m
[32m+[m[32m          break;[m
[32m+[m[32m        }[m
[32m+[m[32m      }[m
[32m+[m
[32m+[m[32m      // Incomplete charset (malformed)[m
[32m+[m[32m      if (i === str.length)[m
[32m+[m[32m        return;[m
[32m+[m
[32m+[m[32m      charset = str.slice(charsetStart, i);[m
[32m+[m[32m      ++i; // Skip over the '\''[m
[32m+[m
[32m+[m[32m      //~ const langStart = ++i;[m
[32m+[m[32m      // Parse language name[m
[32m+[m[32m      for (; i < str.length; ++i) {[m
[32m+[m[32m        const code = str.charCodeAt(i);[m
[32m+[m[32m        if (code === 39/* '\'' */)[m
[32m+[m[32m          break;[m
[32m+[m[32m      }[m
[32m+[m
[32m+[m[32m      // Incomplete language (malformed)[m
[32m+[m[32m      if (i === str.length)[m
[32m+[m[32m        return;[m
[32m+[m
[32m+[m[32m      //~ lang = str.slice(langStart, i);[m
[32m+[m[32m      ++i; // Skip over the '\''[m
[32m+[m
[32m+[m[32m      // No value (malformed)[m
[32m+[m[32m      if (i === str.length)[m
[32m+[m[32m        return;[m
[32m+[m
[32m+[m[32m      valueStart = i;[m
[32m+[m
[32m+[m[32m      let encode = 0;[m
[32m+[m[32m      // Parse value[m
[32m+[m[32m      for (; i < str.length; ++i) {[m
[32m+[m[32m        const code = str.charCodeAt(i);[m
[32m+[m[32m        if (EXTENDED_VALUE[code] !== 1) {[m
[32m+[m[32m          if (code === 37/* '%' */) {[m
[32m+[m[32m            let hexUpper;[m
[32m+[m[32m            let hexLower;[m
[32m+[m[32m            if (i + 2 < str.length[m
[32m+[m[32m                && (hexUpper = HEX_VALUES[str.charCodeAt(i + 1)]) !== -1[m
[32m+[m[32m                && (hexLower = HEX_VALUES[str.charCodeAt(i + 2)]) !== -1) {[m
[32m+[m[32m              const byteVal = (hexUpper << 4) + hexLower;[m
[32m+[m[32m              value += str.slice(valueStart, i);[m
[32m+[m[32m              value += String.fromCharCode(byteVal);[m
[32m+[m[32m              i += 2;[m
[32m+[m[32m              valueStart = i + 1;[m
[32m+[m[32m              if (byteVal >= 128)[m
[32m+[m[32m                encode = 2;[m
[32m+[m[32m              else if (encode === 0)[m
[32m+[m[32m                encode = 1;[m
[32m+[m[32m              continue;[m
[32m+[m[32m            }[m
[32m+[m[32m            // '%' disallowed in non-percent encoded contexts (malformed)[m
[32m+[m[32m            return;[m
[32m+[m[32m          }[m
[32m+[m[32m          break;[m
[32m+[m[32m        }[m
[32m+[m[32m      }[m
[32m+[m
[32m+[m[32m      value += str.slice(valueStart, i);[m
[32m+[m[32m      value = convertToUTF8(value, charset, encode);[m
[32m+[m[32m      if (value === undefined)[m
[32m+[m[32m        return;[m
[32m+[m[32m    } else {[m
[32m+[m[32m      // Non-extended value[m
[32m+[m
[32m+[m[32m      ++i; // Skip over '='[m
[32m+[m
[32m+[m[32m      // No value (malformed)[m
[32m+[m[32m      if (i === str.length)[m
[32m+[m[32m        return;[m
[32m+[m
[32m+[m[32m      if (str.charCodeAt(i) === 34/* '"' */) {[m
[32m+[m[32m        valueStart = ++i;[m
[32m+[m[32m        let escaping = false;[m
[32m+[m[32m        // Parse quoted value[m
[32m+[m[32m        for (; i < str.length; ++i) {[m
[32m+[m[32m          const code = str.charCodeAt(i);[m
[32m+[m[32m          if (code === 92/* '\\' */) {[m
[32m+[m[32m            if (escaping) {[m
[32m+[m[32m              valueStart = i;[m
[32m+[m[32m              escaping = false;[m
[32m+[m[32m            } else {[m
[32m+[m[32m              value += str.slice(valueStart, i);[m
[32m+[m[32m              escaping = true;[m
[32m+[m[32m            }[m
[32m+[m[32m            continue;[m
[32m+[m[32m          }[m
[32m+[m[32m          if (code === 34/* '"' */) {[m
[32m+[m[32m            if (escaping) {[m
[32m+[m[32m              valueStart = i;[m
[32m+[m[32m              escaping = false;[m
[32m+[m[32m              continue;[m
[32m+[m[32m            }[m
[32m+[m[32m            value += str.slice(valueStart, i);[m
[32m+[m[32m            break;[m
[32m+[m[32m          }[m
[32m+[m[32m          if (escaping) {[m
[32m+[m[32m            valueStart = i - 1;[m
[32m+[m[32m            escaping = false;[m
[32m+[m[32m          }[m
[32m+[m[32m          // Invalid unescaped quoted character (malformed)[m
[32m+[m[32m          if (QDTEXT[code] !== 1)[m
[32m+[m[32m            return;[m
[32m+[m[32m        }[m
[32m+[m
[32m+[m[32m        // No end quote (malformed)[m
[32m+[m[32m        if (i === str.length)[m
[32m+[m[32m          return;[m
[32m+[m
[32m+[m[32m        ++i; // Skip over double quote[m
[32m+[m[32m      } else {[m
[32m+[m[32m        valueStart = i;[m
[32m+[m[32m        // Parse unquoted value[m
[32m+[m[32m        for (; i < str.length; ++i) {[m
[32m+[m[32m          const code = str.charCodeAt(i);[m
[32m+[m[32m          if (TOKEN[code] !== 1) {[m
[32m+[m[32m            // No value (malformed)[m
[32m+[m[32m            if (i === valueStart)[m
[32m+[m[32m              return;[m
[32m+[m[32m            break;[m
[32m+[m[32m          }[m
[32m+[m[32m        }[m
[32m+[m[32m        value = str.slice(valueStart, i);[m
[32m+[m[32m      }[m
[32m+[m
[32m+[m[32m      value = defDecoder(value, 2);[m
[32m+[m[32m      if (value === undefined)[m
[32m+[m[32m        return;[m
[32m+[m[32m    }[m
[32m+[m
[32m+[m[32m    name = name.toLowerCase();[m
[32m+[m[32m    if (params[name] === undefined)[m
[32m+[m[32m      params[name] = value;[m
[32m+[m[32m  }[m
[32m+[m
[32m+[m[32m  return params;[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32mfunction getDecoder(charset) {[m
[32m+[m[32m  let lc;[m
[32m+[m[32m  while (true) {[m
[32m+[m[32m    switch (charset) {[m
[32m+[m[32m      case 'utf-8':[m
[32m+[m[32m      case 'utf8':[m
[32m+[m[32m        return decoders.utf8;[m
[32m+[m[32m      case 'latin1':[m
[32m+[m[32m      case 'ascii': // TODO: Make these a separate, strict decoder?[m
[32m+[m[32m      case 'us-ascii':[m
[32m+[m[32m      case 'iso-8859-1':[m
[32m+[m[32m      case 'iso8859-1':[m
[32m+[m[32m      case 'iso88591':[m
[32m+[m[32m      case 'iso_8859-1':[m
[32m+[m[32m      case 'windows-1252':[m
[32m+[m[32m      case 'iso_8859-1:1987':[m
[32m+[m[32m      case 'cp1252':[m
[32m+[m[32m      case 'x-cp1252':[m
[32m+[m[32m        return decoders.latin1;[m
[32m+[m[32m      case 'utf16le':[m
[32m+[m[32m      case 'utf-16le':[m
[32m+[m[32m      case 'ucs2':[m
[32m+[m[32m      case 'ucs-2':[m
[32m+[m[32m        return decoders.utf16le;[m
[32m+[m[32m      case 'base64':[m
[32m+[m[32m        return decoders.base64;[m
[32m+[m[32m      default:[m
[32m+[m[32m        if (lc === undefined) {[m
[32m+[m[32m          lc = true;[m
[32m+[m[32m          charset = charset.toLowerCase();[m
[32m+[m[32m          continue;[m
[32m+[m[32m        }[m
[32m+[m[32m        return decoders.other.bind(charset);[m
[32m+[m[32m    }[m
[32m+[m[32m  }[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32mconst decoders = {[m
[32m+[m[32m  utf8: (data, hint) => {[m
[32m+[m[32m    if (data.length === 0)[m
[32m+[m[32m      return '';[m
[32m+[m[32m    if (typeof data === 'string') {[m
[32m+[m[32m      // If `data` never had any percent-encoded bytes or never had any that[m
[32m+[m[32m      // were outside of the ASCII range, then we can safely just return the[m
[32m+[m[32m      // input since UTF-8 is ASCII compatible[m
[32m+[m[32m      if (hint < 2)[m
[32m+[m[32m        return data;[m
[32m+[m
[32m+[m[32m      data = Buffer.from(data, 'latin1');[m
[32m+[m[32m    }[m
[32m+[m[32m    return data.utf8Slice(0, data.length);[m
[32m+[m[32m  },[m
[32m+[m
[32m+[m[32m  latin1: (data, hint) => {[m
[32m+[m[32m    if (data.length === 0)[m
[32m+[m[32m      return '';[m
[32m+[m[32m    if (typeof data === 'string')[m
[32m+[m[32m      return data;[m
[32m+[m[32m    return data.latin1Slice(0, data.length);[m
[32m+[m[32m  },[m
[32m+[m
[32m+[m[32m  utf16le: (data, hint) => {[m
[32m+[m[32m    if (data.length === 0)[m
[32m+[m[32m      return '';[m
[32m+[m[32m    if (typeof data === 'string')[m
[32m+[m[32m      data = Buffer.from(data, 'latin1');[m
[32m+[m[32m    return data.ucs2Slice(0, data.length);[m
[32m+[m[32m  },[m
[32m+[m
[32m+[m[32m  base64: (data, hint) => {[m
[32m+[m[32m    if (data.length === 0)[m
[32m+[m[32m      return '';[m
[32m+[m[32m    if (typeof data === 'string')[m
[32m+[m[32m      data = Buffer.from(data, 'latin1');[m
[32m+[m[32m    return data.base64Slice(0, data.length);[m
[32m+[m[32m  },[m
[32m+[m
[32m+[m[32m  other: (data, hint) => {[m
[32m+[m[32m    if (data.length === 0)[m
[32m+[m[32m      return '';[m
[32m+[m[32m    if (typeof data === 'string')[m
[32m+[m[32m      data = Buffer.from(data, 'latin1');[m
[32m+[m[32m    try {[m
[32m+[m[32m      const decoder = new TextDecoder(this);[m
[32m+[m[32m      return decoder.decode(data);[m
[32m+[m[32m    } catch {}[m
[32m+[m[32m  },[m
[32m+[m[32m};[m
[32m+[m
[32m+[m[32mfunction convertToUTF8(data, charset, hint) {[m
[32m+[m[32m  const decode = getDecoder(charset);[m
[32m+[m[32m  if (decode)[m
[32m+[m[32m    return decode(data, hint);[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32mfunction basename(path) {[m
[32m+[m[32m  if (typeof path !== 'string')[m
[32m+[m[32m    return '';[m
[32m+[m[32m  for (let i = path.length - 1; i >= 0; --i) {[m
[32m+[m[32m    switch (path.charCodeAt(i)) {[m
[32m+[m[32m      case 0x2F: // '/'[m
[32m+[m[32m      case 0x5C: // '\'[m
[32m+[m[32m        path = path.slice(i + 1);[m
[32m+[m[32m        return (path === '..' || path === '.' ? '' : path);[m
[32m+[m[32m    }[m
[32m+[m[32m  }[m
[32m+[m[32m  return (path === '..' || path === '.' ? '' : path);[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32mconst TOKEN = [[m
[32m+[m[32m  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,[m
[32m+[m[32m  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,[m
[32m+[m[32m  0, 1, 0, 1, 1, 1, 1, 1, 0, 0, 1, 1, 0, 1, 1, 0,[m
[32m+[m[32m  1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0,[m
[32m+[m[32m  0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,[m
[32m+[m[32m  1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1,[m
[32m+[m[32m  1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,[m
[32m+[m[32m  1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0, 1, 0,[m
[32m+[m[32m  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,[m
[32m+[m[32m  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,[m
[32m+[m[32m  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,[m
[32m+[m[32m  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,[m
[32m+[m[32m  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,[m
[32m+[m[32m  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,[m
[32m+[m[32m  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,[m
[32m+[m[32m  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,[m
[32m+[m[32m];[m
[32m+[m
[32m+[m[32mconst QDTEXT = [[m
[32m+[m[32m  0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0,[m
[32m+[m[32m  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,[m
[32m+[m[32m  1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,[m
[32m+[m[32m  1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,[m
[32m+[m[32m  1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,[m
[32m+[m[32m  1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1,[m
[32m+[m[32m  1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,[m
[32m+[m[32m  1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0,[m
[32m+[m[32m  1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,[m
[32m+[m[32m  1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,[m
[32m+[m[32m  1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,[m
[32m+[m[32m  1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,[m
[32m+[m[32m  1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,[m
[32m+[m[32m  1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,[m
[32m+[m[32m  1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,[m
[32m+[m[32m  1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,[m
[32m+[m[32m];[m
[32m+[m
[32m+[m[32mconst CHARSET = [[m
[32m+[m[32m  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,[m
[32m+[m[32m  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,[m
[32m+[m[32m  0, 1, 0, 1, 1, 1, 1, 0, 0, 0, 0, 1, 0, 1, 0, 0,[m
[32m+[m[32m  1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0,[m
[32m+[m[32m  0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,[m
[32m+[m[32m  1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1,[m
[32m+[m[32m  1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,[m
[32m+[m[32m  1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 0,[m
[32m+[m[32m  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,[m
[32m+[m[32m  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,[m
[32m+[m[32m  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,[m
[32m+[m[32m  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,[m
[32m+[m[32m  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,[m
[32m+[m[32m  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,[m
[32m+[m[32m  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,[m
[32m+[m[32m  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,[m
[32m+[m[32m];[m
[32m+[m
[32m+[m[32mconst EXTENDED_VALUE = [[m
[32m+[m[32m  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,[m
[32m+[m[32m  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,[m
[32m+[m[32m  0, 1, 0, 1, 1, 0, 1, 0, 0, 0, 0, 1, 0, 1, 1, 0,[m
[32m+[m[32m  1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0,[m
[32m+[m[32m  0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,[m
[32m+[m[32m  1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1,[m
[32m+[m[32m  1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,[m
[32m+[m[32m  1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0, 1, 0,[m
[32m+[m[32m  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,[m
[32m+[m[32m  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,[m
[32m+[m[32m  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,[m
[32m+[m[32m  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,[m
[32m+[m[32m  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,[m
[32m+[m[32m  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,[m
[32m+[m[32m  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,[m
[32m+[m[32m  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,[m
[32m+[m[32m];[m
[32m+[m
[32m+[m[32m/* eslint-disable no-multi-spaces */[m
[32m+[m[32mconst HEX_VALUES = [[m
[32m+[m[32m  -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,[m
[32m+[m[32m  -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,[m
[32m+[m[32m  -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,[m
[32m+[m[32m   0,  1,  2,  3,  4,  5,  6,  7,  8,  9, -1, -1, -1, -1, -1, -1,[m
[32m+[m[32m  -1, 10, 11, 12, 13, 14, 15, -1, -1, -1, -1, -1, -1, -1, -1, -1,[m
[32m+[m[32m  -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,[m
[32m+[m[32m  -1, 10, 11, 12, 13, 14, 15, -1, -1, -1, -1, -1, -1, -1, -1, -1,[m
[32m+[m[32m  -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,[m
[32m+[m[32m  -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,[m
[32m+[m[32m  -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,[m
[32m+[m[32m  -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,[m
[32m+[m[32m  -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,[m
[32m+[m[32m  -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,[m
[32m+[m[32m  -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,[m
[32m+[m[32m  -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,[m
[32m+[m[32m  -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,[m
[32m+[m[32m];[m
[32m+[m[32m/* eslint-enable no-multi-spaces */[m
[32m+[m
[32m+[m[32mmodule.exports = {[m
[32m+[m[32m  basename,[m
[32m+[m[32m  convertToUTF8,[m
[32m+[m[32m  getDecoder,[m
[32m+[m[32m  parseContentType,[m
[32m+[m[32m  parseDisposition,[m
[32m+[m[32m};[m
[1mdiff --git a/node_modules/busboy/package.json b/node_modules/busboy/package.json[m
[1mnew file mode 100644[m
[1mindex 0000000..ac2577f[m
[1m--- /dev/null[m
[1m+++ b/node_modules/busboy/package.json[m
[36m@@ -0,0 +1,22 @@[m
[32m+[m[32m{ "name": "busboy",[m
[32m+[m[32m  "version": "1.6.0",[m
[32m+[m[32m  "author": "Brian White <mscdex@mscdex.net>",[m
[32m+[m[32m  "description": "A streaming parser for HTML form data for node.js",[m
[32m+[m[32m  "main": "./lib/index.js",[m
[32m+[m[32m  "dependencies": {[m
[32m+[m[32m    "streamsearch": "^1.1.0"[m
[32m+[m[32m  },[m
[32m+[m[32m  "devDependencies": {[m
[32m+[m[32m    "@mscdex/eslint-config": "^1.1.0",[m
[32m+[m[32m    "eslint": "^7.32.0"[m
[32m+[m[32m  },[m
[32m+[m[32m  "scripts": {[m
[32m+[m[32m    "test": "node test/test.js",[m
[32m+[m[32m    "lint": "eslint --cache --report-unused-disable-directives --ext=.js .eslintrc.js lib test bench",[m
[32m+[m[32m    "lint:fix": "npm run lint -- --fix"[m
[32m+[m[32m  },[m
[32m+[m[32m  "engines": { "node": ">=10.16.0" },[m
[32m+[m[32m  "keywords": [ "uploads", "forms", "multipart", "form-data" ],[m
[32m+[m[32m  "licenses": [ { "type": "MIT", "url": "http://github.com/mscdex/busboy/raw/master/LICENSE" } ],[m
[32m+[m[32m  "repository": { "type": "git", "url": "http://github.com/mscdex/busboy.git" }[m
[32m+[m[32m}[m
[1mdiff --git a/node_modules/busboy/test/common.js b/node_modules/busboy/test/common.js[m
[1mnew file mode 100644[m
[1mindex 0000000..fb82ad8[m
[1m--- /dev/null[m
[1m+++ b/node_modules/busboy/test/common.js[m
[36m@@ -0,0 +1,109 @@[m
[32m+[m[32m'use strict';[m
[32m+[m
[32m+[m[32mconst assert = require('assert');[m
[32m+[m[32mconst { inspect } = require('util');[m
[32m+[m
[32m+[m[32mconst mustCallChecks = [];[m
[32m+[m
[32m+[m[32mfunction noop() {}[m
[32m+[m
[32m+[m[32mfunction runCallChecks(exitCode) {[m
[32m+[m[32m  if (exitCode !== 0) return;[m
[32m+[m
[32m+[m[32m  const failed = mustCallChecks.filter((context) => {[m
[32m+[m[32m    if ('minimum' in context) {[m
[32m+[m[32m      context.messageSegment = `at least ${context.minimum}`;[m
[32m+[m[32m      return context.actual < context.minimum;[m
[32m+[m[32m    }[m
[32m+[m[32m    context.messageSegment = `exactly ${context.exact}`;[m
[32m+[m[32m    return context.actual !== context.exact;[m
[32m+[m[32m  });[m
[32m+[m
[32m+[m[32m  failed.forEach((context) => {[m
[32m+[m[32m    console.error('Mismatched %s function calls. Expected %s, actual %d.',[m
[32m+[m[32m                  context.name,[m
[32m+[m[32m                  context.messageSegment,[m
[32m+[m[32m                  context.actual);[m
[32m+[m[32m    console.error(context.stack.split('\n').slice(2).join('\n'));[m
[32m+[m[32m  });[m
[32m+[m
[32m+[m[32m  if (failed.length)[m
[32m+[m[32m    process.exit(1);[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32mfunction mustCall(fn, exact) {[m
[32m+[m[32m  return _mustCallInner(fn, exact, 'exact');[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32mfunction mustCallAtLeast(fn, minimum) {[m
[32m+[m[32m  return _mustCallInner(fn, minimum, 'minimum');[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32mfunction _mustCallInner(fn, criteria = 1, field) {[m
[32m+[m[32m  if (process._exiting)[m
[32m+[m[32m    throw new Error('Cannot use common.mustCall*() in process exit handler');[m
[32m+[m
[32m+[m[32m  if (typeof fn === 'number') {[m
[32m+[m[32m    criteria = fn;[m
[32m+[m[32m    fn = noop;[m
[32m+[m[32m  } else if (fn === undefined) {[m
[32m+[m[32m    fn = noop;[m
[32m+[m[32m  }[m
[32m+[m
[32m+[m[32m  if (typeof criteria !== 'number')[m
[32m+[m[32m    throw new TypeError(`Invalid ${field} value: ${criteria}`);[m
[32m+[m
[32m+[m[32m  const context = {[m
[32m+[m[32m    [field]: criteria,[m
[32m+[m[32m    actual: 0,[m
[32m+[m[32m    stack: inspect(new Error()),[m
[32m+[m[32m    name: fn.name || '<anonymous>'[m
[32m+[m[32m  };[m
[32m+[m
[32m+[m[32m  // Add the exit listener only once to avoid listener leak warnings[m
[32m+[m[32m  if (mustCallChecks.length === 0)[m
[32m+[m[32m    process.on('exit', runCallChecks);[m
[32m+[m
[32m+[m[32m  mustCallChecks.push(context);[m
[32m+[m
[32m+[m[32m  function wrapped(...args) {[m
[32m+[m[32m    ++context.actual;[m
[32m+[m[32m    return fn.call(this, ...args);[m
[32m+[m[32m  }[m
[32m+[m[32m  // TODO: remove origFn?[m
[32m+[m[32m  wrapped.origFn = fn;[m
[32m+[m
[32m+[m[32m  return wrapped;[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32mfunction getCallSite(top) {[m
[32m+[m[32m  const originalStackFormatter = Error.prepareStackTrace;[m
[32m+[m[32m  Error.prepareStackTrace = (err, stack) =>[m
[32m+[m[32m    `${stack[0].getFileName()}:${stack[0].getLineNumber()}`;[m
[32m+[m[32m  const err = new Error();[m
[32m+[m[32m  Error.captureStackTrace(err, top);[m
[32m+[m[32m  // With the V8 Error API, the stack is not formatted until it is accessed[m
[32m+[m[32m  // eslint-disable-next-line no-unused-expressions[m
[32m+[m[32m  err.stack;[m
[32m+[m[32m  Error.prepareStackTrace = originalStackFormatter;[m
[32m+[m[32m  return err.stack;[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32mfunction mustNotCall(msg) {[m
[32m+[m[32m  const callSite = getCallSite(mustNotCall);[m
[32m+[m[32m  return function mustNotCall(...args) {[m
[32m+[m[32m    args = args.map(inspect).join(', ');[m
[32m+[m[32m    const argsInfo = (args.length > 0[m
[32m+[m[32m                      ? `\ncalled with arguments: ${args}`[m
[32m+[m[32m                      : '');[m
[32m+[m[32m    assert.fail([m
[32m+[m[32m      `${msg || 'function should not have been called'} at ${callSite}`[m
[32m+[m[32m        + argsInfo);[m
[32m+[m[32m  };[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32mmodule.exports = {[m
[32m+[m[32m  mustCall,[m
[32m+[m[32m  mustCallAtLeast,[m
[32m+[m[32m  mustNotCall,[m
[32m+[m[32m};[m
[1mdiff --git a/node_modules/busboy/test/test-types-multipart-charsets.js b/node_modules/busboy/test/test-types-multipart-charsets.js[m
[1mnew file mode 100644[m
[1mindex 0000000..ed9c38a[m
[1m--- /dev/null[m
[1m+++ b/node_modules/busboy/test/test-types-multipart-charsets.js[m
[36m@@ -0,0 +1,94 @@[m
[32m+[m[32m'use strict';[m
[32m+[m
[32m+[m[32mconst assert = require('assert');[m
[32m+[m[32mconst { inspect } = require('util');[m
[32m+[m
[32m+[m[32mconst { mustCall } = require(`${__dirname}/common.js`);[m
[32m+[m
[32m+[m[32mconst busboy = require('..');[m
[32m+[m
[32m+[m[32mconst input = Buffer.from([[m
[32m+[m[32m  '-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m 'Content-Disposition: form-data; '[m
[32m+[m[32m   + 'name="upload_file_0"; filename="テスト.dat"',[m
[32m+[m[32m 'Content-Type: application/octet-stream',[m
[32m+[m[32m '',[m
[32m+[m[32m 'A'.repeat(1023),[m
[32m+[m[32m '-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k--'[m
[32m+[m[32m].join('\r\n'));[m
[32m+[m[32mconst boundary = '---------------------------paZqsnEHRufoShdX6fh0lUhXBP4k';[m
[32m+[m[32mconst expected = [[m
[32m+[m[32m  { type: 'file',[m
[32m+[m[32m    name: 'upload_file_0',[m
[32m+[m[32m    data: Buffer.from('A'.repeat(1023)),[m
[32m+[m[32m    info: {[m
[32m+[m[32m      filename: 'テスト.dat',[m
[32m+[m[32m      encoding: '7bit',[m
[32m+[m[32m      mimeType: 'application/octet-stream',[m
[32m+[m[32m    },[m
[32m+[m[32m    limited: false,[m
[32m+[m[32m  },[m
[32m+[m[32m];[m
[32m+[m[32mconst bb = busboy({[m
[32m+[m[32m  defParamCharset: 'utf8',[m
[32m+[m[32m  headers: {[m
[32m+[m[32m    'content-type': `multipart/form-data; boundary=${boundary}`,[m
[32m+[m[32m  }[m
[32m+[m[32m});[m
[32m+[m[32mconst results = [];[m
[32m+[m
[32m+[m[32mbb.on('field', (name, val, info) => {[m
[32m+[m[32m  results.push({ type: 'field', name, val, info });[m
[32m+[m[32m});[m
[32m+[m
[32m+[m[32mbb.on('file', (name, stream, info) => {[m
[32m+[m[32m  const data = [];[m
[32m+[m[32m  let nb = 0;[m
[32m+[m[32m  const file = {[m
[32m+[m[32m    type: 'file',[m
[32m+[m[32m    name,[m
[32m+[m[32m    data: null,[m
[32m+[m[32m    info,[m
[32m+[m[32m    limited: false,[m
[32m+[m[32m  };[m
[32m+[m[32m  results.push(file);[m
[32m+[m[32m  stream.on('data', (d) => {[m
[32m+[m[32m    data.push(d);[m
[32m+[m[32m    nb += d.length;[m
[32m+[m[32m  }).on('limit', () => {[m
[32m+[m[32m    file.limited = true;[m
[32m+[m[32m  }).on('close', () => {[m
[32m+[m[32m    file.data = Buffer.concat(data, nb);[m
[32m+[m[32m    assert.strictEqual(stream.truncated, file.limited);[m
[32m+[m[32m  }).once('error', (err) => {[m
[32m+[m[32m    file.err = err.message;[m
[32m+[m[32m  });[m
[32m+[m[32m});[m
[32m+[m
[32m+[m[32mbb.on('error', (err) => {[m
[32m+[m[32m  results.push({ error: err.message });[m
[32m+[m[32m});[m
[32m+[m
[32m+[m[32mbb.on('partsLimit', () => {[m
[32m+[m[32m  results.push('partsLimit');[m
[32m+[m[32m});[m
[32m+[m
[32m+[m[32mbb.on('filesLimit', () => {[m
[32m+[m[32m  results.push('filesLimit');[m
[32m+[m[32m});[m
[32m+[m
[32m+[m[32mbb.on('fieldsLimit', () => {[m
[32m+[m[32m  results.push('fieldsLimit');[m
[32m+[m[32m});[m
[32m+[m
[32m+[m[32mbb.on('close', mustCall(() => {[m
[32m+[m[32m  assert.deepStrictEqual([m
[32m+[m[32m    results,[m
[32m+[m[32m    expected,[m
[32m+[m[32m    'Results mismatch.\n'[m
[32m+[m[32m      + `Parsed: ${inspect(results)}\n`[m
[32m+[m[32m      + `Expected: ${inspect(expected)}`[m
[32m+[m[32m  );[m
[32m+[m[32m}));[m
[32m+[m
[32m+[m[32mbb.end(input);[m
[1mdiff --git a/node_modules/busboy/test/test-types-multipart-stream-pause.js b/node_modules/busboy/test/test-types-multipart-stream-pause.js[m
[1mnew file mode 100644[m
[1mindex 0000000..df7268a[m
[1m--- /dev/null[m
[1m+++ b/node_modules/busboy/test/test-types-multipart-stream-pause.js[m
[36m@@ -0,0 +1,102 @@[m
[32m+[m[32m'use strict';[m
[32m+[m
[32m+[m[32mconst assert = require('assert');[m
[32m+[m[32mconst { randomFillSync } = require('crypto');[m
[32m+[m[32mconst { inspect } = require('util');[m
[32m+[m
[32m+[m[32mconst busboy = require('..');[m
[32m+[m
[32m+[m[32mconst { mustCall } = require('./common.js');[m
[32m+[m
[32m+[m[32mconst BOUNDARY = 'u2KxIV5yF1y+xUspOQCCZopaVgeV6Jxihv35XQJmuTx8X3sh';[m
[32m+[m
[32m+[m[32mfunction formDataSection(key, value) {[m
[32m+[m[32m  return Buffer.from([m
[32m+[m[32m    `\r\n--${BOUNDARY}`[m
[32m+[m[32m      + `\r\nContent-Disposition: form-data; name="${key}"`[m
[32m+[m[32m      + `\r\n\r\n${value}`[m
[32m+[m[32m  );[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32mfunction formDataFile(key, filename, contentType) {[m
[32m+[m[32m  const buf = Buffer.allocUnsafe(100000);[m
[32m+[m[32m  return Buffer.concat([[m
[32m+[m[32m    Buffer.from(`\r\n--${BOUNDARY}\r\n`),[m
[32m+[m[32m    Buffer.from(`Content-Disposition: form-data; name="${key}"`[m
[32m+[m[32m                  + `; filename="${filename}"\r\n`),[m
[32m+[m[32m    Buffer.from(`Content-Type: ${contentType}\r\n\r\n`),[m
[32m+[m[32m    randomFillSync(buf)[m
[32m+[m[32m  ]);[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32mconst reqChunks = [[m
[32m+[m[32m  Buffer.concat([[m
[32m+[m[32m    formDataFile('file', 'file.bin', 'application/octet-stream'),[m
[32m+[m[32m    formDataSection('foo', 'foo value'),[m
[32m+[m[32m  ]),[m
[32m+[m[32m  formDataSection('bar', 'bar value'),[m
[32m+[m[32m  Buffer.from(`\r\n--${BOUNDARY}--\r\n`)[m
[32m+[m[32m];[m
[32m+[m[32mconst bb = busboy({[m
[32m+[m[32m  headers: {[m
[32m+[m[32m    'content-type': `multipart/form-data; boundary=${BOUNDARY}`[m
[32m+[m[32m  }[m
[32m+[m[32m});[m
[32m+[m[32mconst expected = [[m
[32m+[m[32m  { type: 'file',[m
[32m+[m[32m    name: 'file',[m
[32m+[m[32m    info: {[m
[32m+[m[32m      filename: 'file.bin',[m
[32m+[m[32m      encoding: '7bit',[m
[32m+[m[32m      mimeType: 'application/octet-stream',[m
[32m+[m[32m    },[m
[32m+[m[32m  },[m
[32m+[m[32m  { type: 'field',[m
[32m+[m[32m    name: 'foo',[m
[32m+[m[32m    val: 'foo value',[m
[32m+[m[32m    info: {[m
[32m+[m[32m      nameTruncated: false,[m
[32m+[m[32m      valueTruncated: false,[m
[32m+[m[32m      encoding: '7bit',[m
[32m+[m[32m      mimeType: 'text/plain',[m
[32m+[m[32m    },[m
[32m+[m[32m  },[m
[32m+[m[32m  { type: 'field',[m
[32m+[m[32m    name: 'bar',[m
[32m+[m[32m    val: 'bar value',[m
[32m+[m[32m    info: {[m
[32m+[m[32m      nameTruncated: false,[m
[32m+[m[32m      valueTruncated: false,[m
[32m+[m[32m      encoding: '7bit',[m
[32m+[m[32m      mimeType: 'text/plain',[m
[32m+[m[32m    },[m
[32m+[m[32m  },[m
[32m+[m[32m];[m
[32m+[m[32mconst results = [];[m
[32m+[m
[32m+[m[32mbb.on('field', (name, val, info) => {[m
[32m+[m[32m  results.push({ type: 'field', name, val, info });[m
[32m+[m[32m});[m
[32m+[m
[32m+[m[32mbb.on('file', (name, stream, info) => {[m
[32m+[m[32m  results.push({ type: 'file', name, info });[m
[32m+[m[32m  // Simulate a pipe where the destination is pausing (perhaps due to waiting[m
[32m+[m[32m  // for file system write to finish)[m
[32m+[m[32m  setTimeout(() => {[m
[32m+[m[32m    stream.resume();[m
[32m+[m[32m  }, 10);[m
[32m+[m[32m});[m
[32m+[m
[32m+[m[32mbb.on('close', mustCall(() => {[m
[32m+[m[32m  assert.deepStrictEqual([m
[32m+[m[32m    results,[m
[32m+[m[32m    expected,[m
[32m+[m[32m    'Results mismatch.\n'[m
[32m+[m[32m      + `Parsed: ${inspect(results)}\n`[m
[32m+[m[32m      + `Expected: ${inspect(expected)}`[m
[32m+[m[32m  );[m
[32m+[m[32m}));[m
[32m+[m
[32m+[m[32mfor (const chunk of reqChunks)[m
[32m+[m[32m  bb.write(chunk);[m
[32m+[m[32mbb.end();[m
[1mdiff --git a/node_modules/busboy/test/test-types-multipart.js b/node_modules/busboy/test/test-types-multipart.js[m
[1mnew file mode 100644[m
[1mindex 0000000..9755642[m
[1m--- /dev/null[m
[1m+++ b/node_modules/busboy/test/test-types-multipart.js[m
[36m@@ -0,0 +1,1053 @@[m
[32m+[m[32m'use strict';[m
[32m+[m
[32m+[m[32mconst assert = require('assert');[m
[32m+[m[32mconst { inspect } = require('util');[m
[32m+[m
[32m+[m[32mconst busboy = require('..');[m
[32m+[m
[32m+[m[32mconst active = new Map();[m
[32m+[m
[32m+[m[32mconst tests = [[m
[32m+[m[32m  { source: [[m
[32m+[m[32m      ['-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m       'Content-Disposition: form-data; name="file_name_0"',[m
[32m+[m[32m       '',[m
[32m+[m[32m       'super alpha file',[m
[32m+[m[32m       '-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m       'Content-Disposition: form-data; name="file_name_1"',[m
[32m+[m[32m       '',[m
[32m+[m[32m       'super beta file',[m
[32m+[m[32m       '-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m       'Content-Disposition: form-data; '[m
[32m+[m[32m         + 'name="upload_file_0"; filename="1k_a.dat"',[m
[32m+[m[32m       'Content-Type: application/octet-stream',[m
[32m+[m[32m       '',[m
[32m+[m[32m       'A'.repeat(1023),[m
[32m+[m[32m       '-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m       'Content-Disposition: form-data; '[m
[32m+[m[32m         + 'name="upload_file_1"; filename="1k_b.dat"',[m
[32m+[m[32m       'Content-Type: application/octet-stream',[m
[32m+[m[32m       '',[m
[32m+[m[32m       'B'.repeat(1023),[m
[32m+[m[32m       '-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k--'[m
[32m+[m[32m      ].join('\r\n')[m
[32m+[m[32m    ],[m
[32m+[m[32m    boundary: '---------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m    expected: [[m
[32m+[m[32m      { type: 'field',[m
[32m+[m[32m        name: 'file_name_0',[m
[32m+[m[32m        val: 'super alpha file',[m
[32m+[m[32m        info: {[m
[32m+[m[32m          nameTruncated: false,[m
[32m+[m[32m          valueTruncated: false,[m
[32m+[m[32m          encoding: '7bit',[m
[32m+[m[32m          mimeType: 'text/plain',[m
[32m+[m[32m        },[m
[32m+[m[32m      },[m
[32m+[m[32m      { type: 'field',[m
[32m+[m[32m        name: 'file_name_1',[m
[32m+[m[32m        val: 'super beta file',[m
[32m+[m[32m        info: {[m
[32m+[m[32m          nameTruncated: false,[m
[32m+[m[32m          valueTruncated: false,[m
[32m+[m[32m          encoding: '7bit',[m
[32m+[m[32m          mimeType: 'text/plain',[m
[32m+[m[32m        },[m
[32m+[m[32m      },[m
[32m+[m[32m      { type: 'file',[m
[32m+[m[32m        name: 'upload_file_0',[m
[32m+[m[32m        data: Buffer.from('A'.repeat(1023)),[m
[32m+[m[32m        info: {[m
[32m+[m[32m          filename: '1k_a.dat',[m
[32m+[m[32m          encoding: '7bit',[m
[32m+[m[32m          mimeType: 'application/octet-stream',[m
[32m+[m[32m        },[m
[32m+[m[32m        limited: false,[m
[32m+[m[32m      },[m
[32m+[m[32m      { type: 'file',[m
[32m+[m[32m        name: 'upload_file_1',[m
[32m+[m[32m        data: Buffer.from('B'.repeat(1023)),[m
[32m+[m[32m        info: {[m
[32m+[m[32m          filename: '1k_b.dat',[m
[32m+[m[32m          encoding: '7bit',[m
[32m+[m[32m          mimeType: 'application/octet-stream',[m
[32m+[m[32m        },[m
[32m+[m[32m        limited: false,[m
[32m+[m[32m      },[m
[32m+[m[32m    ],[m
[32m+[m[32m    what: 'Fields and files'[m
[32m+[m[32m  },[m
[32m+[m[32m  { source: [[m
[32m+[m[32m      ['------WebKitFormBoundaryTB2MiQ36fnSJlrhY',[m
[32m+[m[32m       'Content-Disposition: form-data; name="cont"',[m
[32m+[m[32m       '',[m
[32m+[m[32m       'some random content',[m
[32m+[m[32m       '------WebKitFormBoundaryTB2MiQ36fnSJlrhY',[m
[32m+[m[32m       'Content-Disposition: form-data; name="pass"',[m
[32m+[m[32m       '',[m
[32m+[m[32m       'some random pass',[m
[32m+[m[32m       '------WebKitFormBoundaryTB2MiQ36fnSJlrhY',[m
[32m+[m[32m       'Content-Disposition: form-data; name=bit',[m
[32m+[m[32m       '',[m
[32m+[m[32m       '2',[m
[32m+[m[32m       '------WebKitFormBoundaryTB2MiQ36fnSJlrhY--'[m
[32m+[m[32m      ].join('\r\n')[m
[32m+[m[32m    ],[m
[32m+[m[32m    boundary: '----WebKitFormBoundaryTB2MiQ36fnSJlrhY',[m
[32m+[m[32m    expected: [[m
[32m+[m[32m      { type: 'field',[m
[32m+[m[32m        name: 'cont',[m
[32m+[m[32m        val: 'some random content',[m
[32m+[m[32m        info: {[m
[32m+[m[32m          nameTruncated: false,[m
[32m+[m[32m          valueTruncated: false,[m
[32m+[m[32m          encoding: '7bit',[m
[32m+[m[32m          mimeType: 'text/plain',[m
[32m+[m[32m        },[m
[32m+[m[32m      },[m
[32m+[m[32m      { type: 'field',[m
[32m+[m[32m        name: 'pass',[m
[32m+[m[32m        val: 'some random pass',[m
[32m+[m[32m        info: {[m
[32m+[m[32m          nameTruncated: false,[m
[32m+[m[32m          valueTruncated: false,[m
[32m+[m[32m          encoding: '7bit',[m
[32m+[m[32m          mimeType: 'text/plain',[m
[32m+[m[32m        },[m
[32m+[m[32m      },[m
[32m+[m[32m      { type: 'field',[m
[32m+[m[32m        name: 'bit',[m
[32m+[m[32m        val: '2',[m
[32m+[m[32m        info: {[m
[32m+[m[32m          nameTruncated: false,[m
[32m+[m[32m          valueTruncated: false,[m
[32m+[m[32m          encoding: '7bit',[m
[32m+[m[32m          mimeType: 'text/plain',[m
[32m+[m[32m        },[m
[32m+[m[32m      },[m
[32m+[m[32m    ],[m
[32m+[m[32m    what: 'Fields only'[m
[32m+[m[32m  },[m
[32m+[m[32m  { source: [[m
[32m+[m[32m      ''[m
[32m+[m[32m    ],[m
[32m+[m[32m    boundary: '----WebKitFormBoundaryTB2MiQ36fnSJlrhY',[m
[32m+[m[32m    expected: [[m
[32m+[m[32m      { error: 'Unexpected end of form' },[m
[32m+[m[32m    ],[m
[32m+[m[32m    what: 'No fields and no files'[m
[32m+[m[32m  },[m
[32m+[m[32m  { source: [[m
[32m+[m[32m      ['-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m       'Content-Disposition: form-data; name="file_name_0"',[m
[32m+[m[32m       '',[m
[32m+[m[32m       'super alpha file',[m
[32m+[m[32m       '-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m       'Content-Disposition: form-data; '[m
[32m+[m[32m         + 'name="upload_file_0"; filename="1k_a.dat"',[m
[32m+[m[32m       'Content-Type: application/octet-stream',[m
[32m+[m[32m       '',[m
[32m+[m[32m       'ABCDEFGHIJKLMNOPQRSTUVWXYZ',[m
[32m+[m[32m       '-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k--'[m
[32m+[m[32m      ].join('\r\n')[m
[32m+[m[32m    ],[m
[32m+[m[32m    boundary: '---------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m    limits: {[m
[32m+[m[32m      fileSize: 13,[m
[32m+[m[32m      fieldSize: 5[m
[32m+[m[32m    },[m
[32m+[m[32m    expected: [[m
[32m+[m[32m      { type: 'field',[m
[32m+[m[32m        name: 'file_name_0',[m
[32m+[m[32m        val: 'super',[m
[32m+[m[32m        info: {[m
[32m+[m[32m          nameTruncated: false,[m
[32m+[m[32m          valueTruncated: true,[m
[32m+[m[32m          encoding: '7bit',[m
[32m+[m[32m          mimeType: 'text/plain',[m
[32m+[m[32m        },[m
[32m+[m[32m      },[m
[32m+[m[32m      { type: 'file',[m
[32m+[m[32m        name: 'upload_file_0',[m
[32m+[m[32m        data: Buffer.from('ABCDEFGHIJKLM'),[m
[32m+[m[32m        info: {[m
[32m+[m[32m          filename: '1k_a.dat',[m
[32m+[m[32m          encoding: '7bit',[m
[32m+[m[32m          mimeType: 'application/octet-stream',[m
[32m+[m[32m        },[m
[32m+[m[32m        limited: true,[m
[32m+[m[32m      },[m
[32m+[m[32m    ],[m
[32m+[m[32m    what: 'Fields and files (limits)'[m
[32m+[m[32m  },[m
[32m+[m[32m  { source: [[m
[32m+[m[32m      ['-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m       'Content-Disposition: form-data; name="file_name_0"',[m
[32m+[m[32m       '',[m
[32m+[m[32m       'super alpha file',[m
[32m+[m[32m       '-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m       'Content-Disposition: form-data; '[m
[32m+[m[32m         + 'name="upload_file_0"; filename="1k_a.dat"',[m
[32m+[m[32m       'Content-Type: application/octet-stream',[m
[32m+[m[32m       '',[m
[32m+[m[32m       'ABCDEFGHIJKLMNOPQRSTUVWXYZ',[m
[32m+[m[32m       '-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k--'[m
[32m+[m[32m      ].join('\r\n')[m
[32m+[m[32m    ],[m
[32m+[m[32m    boundary: '---------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m    limits: {[m
[32m+[m[32m      files: 0[m
[32m+[m[32m    },[m
[32m+[m[32m    expected: [[m
[32m+[m[32m      { type: 'field',[m
[32m+[m[32m        name: 'file_name_0',[m
[32m+[m[32m        val: 'super alpha file',[m
[32m+[m[32m        info: {[m
[32m+[m[32m          nameTruncated: false,[m
[32m+[m[32m          valueTruncated: false,[m
[32m+[m[32m          encoding: '7bit',[m
[32m+[m[32m          mimeType: 'text/plain',[m
[32m+[m[32m        },[m
[32m+[m[32m      },[m
[32m+[m[32m      'filesLimit',[m
[32m+[m[32m    ],[m
[32m+[m[32m    what: 'Fields and files (limits: 0 files)'[m
[32m+[m[32m  },[m
[32m+[m[32m  { source: [[m
[32m+[m[32m      ['-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m       'Content-Disposition: form-data; name="file_name_0"',[m
[32m+[m[32m       '',[m
[32m+[m[32m       'super alpha file',[m
[32m+[m[32m       '-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m       'Content-Disposition: form-data; name="file_name_1"',[m
[32m+[m[32m       '',[m
[32m+[m[32m       'super beta file',[m
[32m+[m[32m       '-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m       'Content-Disposition: form-data; '[m
[32m+[m[32m         + 'name="upload_file_0"; filename="1k_a.dat"',[m
[32m+[m[32m       'Content-Type: application/octet-stream',[m
[32m+[m[32m       '',[m
[32m+[m[32m       'A'.repeat(1023),[m
[32m+[m[32m       '-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m       'Content-Disposition: form-data; '[m
[32m+[m[32m         + 'name="upload_file_1"; filename="1k_b.dat"',[m
[32m+[m[32m       'Content-Type: application/octet-stream',[m
[32m+[m[32m       '',[m
[32m+[m[32m       'B'.repeat(1023),[m
[32m+[m[32m       '-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k--'[m
[32m+[m[32m      ].join('\r\n')[m
[32m+[m[32m    ],[m
[32m+[m[32m    boundary: '---------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m    expected: [[m
[32m+[m[32m      { type: 'field',[m
[32m+[m[32m        name: 'file_name_0',[m
[32m+[m[32m        val: 'super alpha file',[m
[32m+[m[32m        info: {[m
[32m+[m[32m          nameTruncated: false,[m
[32m+[m[32m          valueTruncated: false,[m
[32m+[m[32m          encoding: '7bit',[m
[32m+[m[32m          mimeType: 'text/plain',[m
[32m+[m[32m        },[m
[32m+[m[32m      },[m
[32m+[m[32m      { type: 'field',[m
[32m+[m[32m        name: 'file_name_1',[m
[32m+[m[32m        val: 'super beta file',[m
[32m+[m[32m        info: {[m
[32m+[m[32m          nameTruncated: false,[m
[32m+[m[32m          valueTruncated: false,[m
[32m+[m[32m          encoding: '7bit',[m
[32m+[m[32m          mimeType: 'text/plain',[m
[32m+[m[32m        },[m
[32m+[m[32m      },[m
[32m+[m[32m    ],[m
[32m+[m[32m    events: ['field'],[m
[32m+[m[32m    what: 'Fields and (ignored) files'[m
[32m+[m[32m  },[m
[32m+[m[32m  { source: [[m
[32m+[m[32m      ['-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m       'Content-Disposition: form-data; '[m
[32m+[m[32m         + 'name="upload_file_0"; filename="/tmp/1k_a.dat"',[m
[32m+[m[32m       'Content-Type: application/octet-stream',[m
[32m+[m[32m       '',[m
[32m+[m[32m       'ABCDEFGHIJKLMNOPQRSTUVWXYZ',[m
[32m+[m[32m       '-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m       'Content-Disposition: form-data; '[m
[32m+[m[32m         + 'name="upload_file_1"; filename="C:\\files\\1k_b.dat"',[m
[32m+[m[32m       'Content-Type: application/octet-stream',[m
[32m+[m[32m       '',[m
[32m+[m[32m       'ABCDEFGHIJKLMNOPQRSTUVWXYZ',[m
[32m+[m[32m       '-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m       'Content-Disposition: form-data; '[m
[32m+[m[32m         + 'name="upload_file_2"; filename="relative/1k_c.dat"',[m
[32m+[m[32m       'Content-Type: application/octet-stream',[m
[32m+[m[32m       '',[m
[32m+[m[32m       'ABCDEFGHIJKLMNOPQRSTUVWXYZ',[m
[32m+[m[32m       '-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k--'[m
[32m+[m[32m      ].join('\r\n')[m
[32m+[m[32m    ],[m
[32m+[m[32m    boundary: '---------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m    expected: [[m
[32m+[m[32m      { type: 'file',[m
[32m+[m[32m        name: 'upload_file_0',[m
[32m+[m[32m        data: Buffer.from('ABCDEFGHIJKLMNOPQRSTUVWXYZ'),[m
[32m+[m[32m        info: {[m
[32m+[m[32m          filename: '1k_a.dat',[m
[32m+[m[32m          encoding: '7bit',[m
[32m+[m[32m          mimeType: 'application/octet-stream',[m
[32m+[m[32m        },[m
[32m+[m[32m        limited: false,[m
[32m+[m[32m      },[m
[32m+[m[32m      { type: 'file',[m
[32m+[m[32m        name: 'upload_file_1',[m
[32m+[m[32m        data: Buffer.from('ABCDEFGHIJKLMNOPQRSTUVWXYZ'),[m
[32m+[m[32m        info: {[m
[32m+[m[32m          filename: '1k_b.dat',[m
[32m+[m[32m          encoding: '7bit',[m
[32m+[m[32m          mimeType: 'application/octet-stream',[m
[32m+[m[32m        },[m
[32m+[m[32m        limited: false,[m
[32m+[m[32m      },[m
[32m+[m[32m      { type: 'file',[m
[32m+[m[32m        name: 'upload_file_2',[m
[32m+[m[32m        data: Buffer.from('ABCDEFGHIJKLMNOPQRSTUVWXYZ'),[m
[32m+[m[32m        info: {[m
[32m+[m[32m          filename: '1k_c.dat',[m
[32m+[m[32m          encoding: '7bit',[m
[32m+[m[32m          mimeType: 'application/octet-stream',[m
[32m+[m[32m        },[m
[32m+[m[32m        limited: false,[m
[32m+[m[32m      },[m
[32m+[m[32m    ],[m
[32m+[m[32m    what: 'Files with filenames containing paths'[m
[32m+[m[32m  },[m
[32m+[m[32m  { source: [[m
[32m+[m[32m      ['-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m       'Content-Disposition: form-data; '[m
[32m+[m[32m         + 'name="upload_file_0"; filename="/absolute/1k_a.dat"',[m
[32m+[m[32m       'Content-Type: application/octet-stream',[m
[32m+[m[32m       '',[m
[32m+[m[32m       'ABCDEFGHIJKLMNOPQRSTUVWXYZ',[m
[32m+[m[32m       '-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m       'Content-Disposition: form-data; '[m
[32m+[m[32m         + 'name="upload_file_1"; filename="C:\\absolute\\1k_b.dat"',[m
[32m+[m[32m       'Content-Type: application/octet-stream',[m
[32m+[m[32m       '',[m
[32m+[m[32m       'ABCDEFGHIJKLMNOPQRSTUVWXYZ',[m
[32m+[m[32m       '-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m       'Content-Disposition: form-data; '[m
[32m+[m[32m         + 'name="upload_file_2"; filename="relative/1k_c.dat"',[m
[32m+[m[32m       'Content-Type: application/octet-stream',[m
[32m+[m[32m       '',[m
[32m+[m[32m       'ABCDEFGHIJKLMNOPQRSTUVWXYZ',[m
[32m+[m[32m       '-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k--'[m
[32m+[m[32m      ].join('\r\n')[m
[32m+[m[32m    ],[m
[32m+[m[32m    boundary: '---------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m    preservePath: true,[m
[32m+[m[32m    expected: [[m
[32m+[m[32m      { type: 'file',[m
[32m+[m[32m        name: 'upload_file_0',[m
[32m+[m[32m        data: Buffer.from('ABCDEFGHIJKLMNOPQRSTUVWXYZ'),[m
[32m+[m[32m        info: {[m
[32m+[m[32m          filename: '/absolute/1k_a.dat',[m
[32m+[m[32m          encoding: '7bit',[m
[32m+[m[32m          mimeType: 'application/octet-stream',[m
[32m+[m[32m        },[m
[32m+[m[32m        limited: false,[m
[32m+[m[32m      },[m
[32m+[m[32m      { type: 'file',[m
[32m+[m[32m        name: 'upload_file_1',[m
[32m+[m[32m        data: Buffer.from('ABCDEFGHIJKLMNOPQRSTUVWXYZ'),[m
[32m+[m[32m        info: {[m
[32m+[m[32m          filename: 'C:\\absolute\\1k_b.dat',[m
[32m+[m[32m          encoding: '7bit',[m
[32m+[m[32m          mimeType: 'application/octet-stream',[m
[32m+[m[32m        },[m
[32m+[m[32m        limited: false,[m
[32m+[m[32m      },[m
[32m+[m[32m      { type: 'file',[m
[32m+[m[32m        name: 'upload_file_2',[m
[32m+[m[32m        data: Buffer.from('ABCDEFGHIJKLMNOPQRSTUVWXYZ'),[m
[32m+[m[32m        info: {[m
[32m+[m[32m          filename: 'relative/1k_c.dat',[m
[32m+[m[32m          encoding: '7bit',[m
[32m+[m[32m          mimeType: 'application/octet-stream',[m
[32m+[m[32m        },[m
[32m+[m[32m        limited: false,[m
[32m+[m[32m      },[m
[32m+[m[32m    ],[m
[32m+[m[32m    what: 'Paths to be preserved through the preservePath option'[m
[32m+[m[32m  },[m
[32m+[m[32m  { source: [[m
[32m+[m[32m      ['------WebKitFormBoundaryTB2MiQ36fnSJlrhY',[m
[32m+[m[32m       'Content-Disposition: form-data; name="cont"',[m
[32m+[m[32m       'Content-Type: ',[m
[32m+[m[32m       '',[m
[32m+[m[32m       'some random content',[m
[32m+[m[32m       '------WebKitFormBoundaryTB2MiQ36fnSJlrhY',[m
[32m+[m[32m       'Content-Disposition: ',[m
[32m+[m[32m       '',[m
[32m+[m[32m       'some random pass',[m
[32m+[m[32m       '------WebKitFormBoundaryTB2MiQ36fnSJlrhY--'[m
[32m+[m[32m      ].join('\r\n')[m
[32m+[m[32m    ],[m
[32m+[m[32m    boundary: '----WebKitFormBoundaryTB2MiQ36fnSJlrhY',[m
[32m+[m[32m    expected: [[m
[32m+[m[32m      { type: 'field',[m
[32m+[m[32m        name: 'cont',[m
[32m+[m[32m        val: 'some random content',[m
[32m+[m[32m        info: {[m
[32m+[m[32m          nameTruncated: false,[m
[32m+[m[32m          valueTruncated: false,[m
[32m+[m[32m          encoding: '7bit',[m
[32m+[m[32m          mimeType: 'text/plain',[m
[32m+[m[32m        },[m
[32m+[m[32m      },[m
[32m+[m[32m    ],[m
[32m+[m[32m    what: 'Empty content-type and empty content-disposition'[m
[32m+[m[32m  },[m
[32m+[m[32m  { source: [[m
[32m+[m[32m      ['-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m       'Content-Disposition: form-data; '[m
[32m+[m[32m         + 'name="file"; filename*=utf-8\'\'n%C3%A4me.txt',[m
[32m+[m[32m       'Content-Type: application/octet-stream',[m
[32m+[m[32m       '',[m
[32m+[m[32m       'ABCDEFGHIJKLMNOPQRSTUVWXYZ',[m
[32m+[m[32m       '-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k--'[m
[32m+[m[32m      ].join('\r\n')[m
[32m+[m[32m    ],[m
[32m+[m[32m    boundary: '---------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m    expected: [[m
[32m+[m[32m      { type: 'file',[m
[32m+[m[32m        name: 'file',[m
[32m+[m[32m        data: Buffer.from('ABCDEFGHIJKLMNOPQRSTUVWXYZ'),[m
[32m+[m[32m        info: {[m
[32m+[m[32m          filename: 'näme.txt',[m
[32m+[m[32m          encoding: '7bit',[m
[32m+[m[32m          mimeType: 'application/octet-stream',[m
[32m+[m[32m        },[m
[32m+[m[32m        limited: false,[m
[32m+[m[32m      },[m
[32m+[m[32m    ],[m
[32m+[m[32m    what: 'Unicode filenames'[m
[32m+[m[32m  },[m
[32m+[m[32m  { source: [[m
[32m+[m[32m      ['--asdasdasdasd\r\n',[m
[32m+[m[32m       'Content-Type: text/plain\r\n',[m
[32m+[m[32m       'Content-Disposition: form-data; name="foo"\r\n',[m
[32m+[m[32m       '\r\n',[m
[32m+[m[32m       'asd\r\n',[m
[32m+[m[32m       '--asdasdasdasd--'[m
[32m+[m[32m      ].join(':)')[m
[32m+[m[32m    ],[m
[32m+[m[32m    boundary: 'asdasdasdasd',[m
[32m+[m[32m    expected: [[m
[32m+[m[32m      { error: 'Malformed part header' },[m
[32m+[m[32m      { error: 'Unexpected end of form' },[m
[32m+[m[32m    ],[m
[32m+[m[32m    what: 'Stopped mid-header'[m
[32m+[m[32m  },[m
[32m+[m[32m  { source: [[m
[32m+[m[32m      ['------WebKitFormBoundaryTB2MiQ36fnSJlrhY',[m
[32m+[m[32m       'Content-Disposition: form-data; name="cont"',[m
[32m+[m[32m       'Content-Type: application/json',[m
[32m+[m[32m       '',[m
[32m+[m[32m       '{}',[m
[32m+[m[32m       '------WebKitFormBoundaryTB2MiQ36fnSJlrhY--',[m
[32m+[m[32m      ].join('\r\n')[m
[32m+[m[32m    ],[m
[32m+[m[32m    boundary: '----WebKitFormBoundaryTB2MiQ36fnSJlrhY',[m
[32m+[m[32m    expected: [[m
[32m+[m[32m      { type: 'field',[m
[32m+[m[32m        name: 'cont',[m
[32m+[m[32m        val: '{}',[m
[32m+[m[32m        info: {[m
[32m+[m[32m          nameTruncated: false,[m
[32m+[m[32m          valueTruncated: false,[m
[32m+[m[32m          encoding: '7bit',[m
[32m+[m[32m          mimeType: 'application/json',[m
[32m+[m[32m        },[m
[32m+[m[32m      },[m
[32m+[m[32m    ],[m
[32m+[m[32m    what: 'content-type for fields'[m
[32m+[m[32m  },[m
[32m+[m[32m  { source: [[m
[32m+[m[32m      '------WebKitFormBoundaryTB2MiQ36fnSJlrhY--',[m
[32m+[m[32m    ],[m
[32m+[m[32m    boundary: '----WebKitFormBoundaryTB2MiQ36fnSJlrhY',[m
[32m+[m[32m    expected: [],[m
[32m+[m[32m    what: 'empty form'[m
[32m+[m[32m  },[m
[32m+[m[32m  { source: [[m
[32m+[m[32m      ['-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m       'Content-Disposition: form-data; '[m
[32m+[m[32m         + 'name=upload_file_0; filename="1k_a.dat"',[m
[32m+[m[32m       'Content-Type: application/octet-stream',[m
[32m+[m[32m       'Content-Transfer-Encoding: binary',[m
[32m+[m[32m       '',[m
[32m+[m[32m       '',[m
[32m+[m[32m      ].join('\r\n')[m
[32m+[m[32m    ],[m
[32m+[m[32m    boundary: '---------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m    expected: [[m
[32m+[m[32m      { type: 'file',[m
[32m+[m[32m        name: 'upload_file_0',[m
[32m+[m[32m        data: Buffer.alloc(0),[m
[32m+[m[32m        info: {[m
[32m+[m[32m          filename: '1k_a.dat',[m
[32m+[m[32m          encoding: 'binary',[m
[32m+[m[32m          mimeType: 'application/octet-stream',[m
[32m+[m[32m        },[m
[32m+[m[32m        limited: false,[m
[32m+[m[32m        err: 'Unexpected end of form',[m
[32m+[m[32m      },[m
[32m+[m[32m      { error: 'Unexpected end of form' },[m
[32m+[m[32m    ],[m
[32m+[m[32m    what: 'Stopped mid-file #1'[m
[32m+[m[32m  },[m
[32m+[m[32m  { source: [[m
[32m+[m[32m      ['-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m       'Content-Disposition: form-data; '[m
[32m+[m[32m         + 'name=upload_file_0; filename="1k_a.dat"',[m
[32m+[m[32m       'Content-Type: application/octet-stream',[m
[32m+[m[32m       '',[m
[32m+[m[32m       'a',[m
[32m+[m[32m      ].join('\r\n')[m
[32m+[m[32m    ],[m
[32m+[m[32m    boundary: '---------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m    expected: [[m
[32m+[m[32m      { type: 'file',[m
[32m+[m[32m        name: 'upload_file_0',[m
[32m+[m[32m        data: Buffer.from('a'),[m
[32m+[m[32m        info: {[m
[32m+[m[32m          filename: '1k_a.dat',[m
[32m+[m[32m          encoding: '7bit',[m
[32m+[m[32m          mimeType: 'application/octet-stream',[m
[32m+[m[32m        },[m
[32m+[m[32m        limited: false,[m
[32m+[m[32m        err: 'Unexpected end of form',[m
[32m+[m[32m      },[m
[32m+[m[32m      { error: 'Unexpected end of form' },[m
[32m+[m[32m    ],[m
[32m+[m[32m    what: 'Stopped mid-file #2'[m
[32m+[m[32m  },[m
[32m+[m[32m  { source: [[m
[32m+[m[32m      ['-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m       'Content-Disposition: form-data; '[m
[32m+[m[32m         + 'name="upload_file_0"; filename="notes.txt"',[m
[32m+[m[32m       'Content-Type: text/plain; charset=utf8',[m
[32m+[m[32m       '',[m
[32m+[m[32m       'a',[m
[32m+[m[32m       '-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k--',[m
[32m+[m[32m      ].join('\r\n')[m
[32m+[m[32m    ],[m
[32m+[m[32m    boundary: '---------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m    expected: [[m
[32m+[m[32m      { type: 'file',[m
[32m+[m[32m        name: 'upload_file_0',[m
[32m+[m[32m        data: Buffer.from('a'),[m
[32m+[m[32m        info: {[m
[32m+[m[32m          filename: 'notes.txt',[m
[32m+[m[32m          encoding: '7bit',[m
[32m+[m[32m          mimeType: 'text/plain',[m
[32m+[m[32m        },[m
[32m+[m[32m        limited: false,[m
[32m+[m[32m      },[m
[32m+[m[32m    ],[m
[32m+[m[32m    what: 'Text file with charset'[m
[32m+[m[32m  },[m
[32m+[m[32m  { source: [[m
[32m+[m[32m      ['-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m       'Content-Disposition: form-data; '[m
[32m+[m[32m         + 'name="upload_file_0"; filename="notes.txt"',[m
[32m+[m[32m       'Content-Type: ',[m
[32m+[m[32m       ' text/plain; charset=utf8',[m
[32m+[m[32m       '',[m
[32m+[m[32m       'a',[m
[32m+[m[32m       '-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k--',[m
[32m+[m[32m      ].join('\r\n')[m
[32m+[m[32m    ],[m
[32m+[m[32m    boundary: '---------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m    expected: [[m
[32m+[m[32m      { type: 'file',[m
[32m+[m[32m        name: 'upload_file_0',[m
[32m+[m[32m        data: Buffer.from('a'),[m
[32m+[m[32m        info: {[m
[32m+[m[32m          filename: 'notes.txt',[m
[32m+[m[32m          encoding: '7bit',[m
[32m+[m[32m          mimeType: 'text/plain',[m
[32m+[m[32m        },[m
[32m+[m[32m        limited: false,[m
[32m+[m[32m      },[m
[32m+[m[32m    ],[m
[32m+[m[32m    what: 'Folded header value'[m
[32m+[m[32m  },[m
[32m+[m[32m  { source: [[m
[32m+[m[32m      ['-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m       'Content-Type: text/plain; charset=utf8',[m
[32m+[m[32m       '',[m
[32m+[m[32m       'a',[m
[32m+[m[32m       '-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k--',[m
[32m+[m[32m      ].join('\r\n')[m
[32m+[m[32m    ],[m
[32m+[m[32m    boundary: '---------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m    expected: [],[m
[32m+[m[32m    what: 'No Content-Disposition'[m
[32m+[m[32m  },[m
[32m+[m[32m  { source: [[m
[32m+[m[32m      ['-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m       'Content-Disposition: form-data; name="file_name_0"',[m
[32m+[m[32m       '',[m
[32m+[m[32m       'a'.repeat(64 * 1024),[m
[32m+[m[32m       '-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m       'Content-Disposition: form-data; '[m
[32m+[m[32m         + 'name="upload_file_0"; filename="notes.txt"',[m
[32m+[m[32m       'Content-Type: ',[m
[32m+[m[32m       ' text/plain; charset=utf8',[m
[32m+[m[32m       '',[m
[32m+[m[32m       'bc',[m
[32m+[m[32m       '-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k--',[m
[32m+[m[32m      ].join('\r\n')[m
[32m+[m[32m    ],[m
[32m+[m[32m    boundary: '---------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m    limits: {[m
[32m+[m[32m      fieldSize: Infinity,[m
[32m+[m[32m    },[m
[32m+[m[32m    expected: [[m
[32m+[m[32m      { type: 'file',[m
[32m+[m[32m        name: 'upload_file_0',[m
[32m+[m[32m        data: Buffer.from('bc'),[m
[32m+[m[32m        info: {[m
[32m+[m[32m          filename: 'notes.txt',[m
[32m+[m[32m          encoding: '7bit',[m
[32m+[m[32m          mimeType: 'text/plain',[m
[32m+[m[32m        },[m
[32m+[m[32m        limited: false,[m
[32m+[m[32m      },[m
[32m+[m[32m    ],[m
[32m+[m[32m    events: [ 'file' ],[m
[32m+[m[32m    what: 'Skip field parts if no listener'[m
[32m+[m[32m  },[m
[32m+[m[32m  { source: [[m
[32m+[m[32m      ['-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m       'Content-Disposition: form-data; name="file_name_0"',[m
[32m+[m[32m       '',[m
[32m+[m[32m       'a',[m
[32m+[m[32m       '-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m       'Content-Disposition: form-data; '[m
[32m+[m[32m         + 'name="upload_file_0"; filename="notes.txt"',[m
[32m+[m[32m       'Content-Type: ',[m
[32m+[m[32m       ' text/plain; charset=utf8',[m
[32m+[m[32m       '',[m
[32m+[m[32m       'bc',[m
[32m+[m[32m       '-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k--',[m
[32m+[m[32m      ].join('\r\n')[m
[32m+[m[32m    ],[m
[32m+[m[32m    boundary: '---------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m    limits: {[m
[32m+[m[32m      parts: 1,[m
[32m+[m[32m    },[m
[32m+[m[32m    expected: [[m
[32m+[m[32m      { type: 'field',[m
[32m+[m[32m        name: 'file_name_0',[m
[32m+[m[32m        val: 'a',[m
[32m+[m[32m        info: {[m
[32m+[m[32m          nameTruncated: false,[m
[32m+[m[32m          valueTruncated: false,[m
[32m+[m[32m          encoding: '7bit',[m
[32m+[m[32m          mimeType: 'text/plain',[m
[32m+[m[32m        },[m
[32m+[m[32m      },[m
[32m+[m[32m      'partsLimit',[m
[32m+[m[32m    ],[m
[32m+[m[32m    what: 'Parts limit'[m
[32m+[m[32m  },[m
[32m+[m[32m  { source: [[m
[32m+[m[32m      ['-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m       'Content-Disposition: form-data; name="file_name_0"',[m
[32m+[m[32m       '',[m
[32m+[m[32m       'a',[m
[32m+[m[32m       '-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m       'Content-Disposition: form-data; name="file_name_1"',[m
[32m+[m[32m       '',[m
[32m+[m[32m       'b',[m
[32m+[m[32m       '-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k--',[m
[32m+[m[32m      ].join('\r\n')[m
[32m+[m[32m    ],[m
[32m+[m[32m    boundary: '---------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m    limits: {[m
[32m+[m[32m      fields: 1,[m
[32m+[m[32m    },[m
[32m+[m[32m    expected: [[m
[32m+[m[32m      { type: 'field',[m
[32m+[m[32m        name: 'file_name_0',[m
[32m+[m[32m        val: 'a',[m
[32m+[m[32m        info: {[m
[32m+[m[32m          nameTruncated: false,[m
[32m+[m[32m          valueTruncated: false,[m
[32m+[m[32m          encoding: '7bit',[m
[32m+[m[32m          mimeType: 'text/plain',[m
[32m+[m[32m        },[m
[32m+[m[32m      },[m
[32m+[m[32m      'fieldsLimit',[m
[32m+[m[32m    ],[m
[32m+[m[32m    what: 'Fields limit'[m
[32m+[m[32m  },[m
[32m+[m[32m  { source: [[m
[32m+[m[32m      ['-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m       'Content-Disposition: form-data; '[m
[32m+[m[32m         + 'name="upload_file_0"; filename="notes.txt"',[m
[32m+[m[32m       'Content-Type: text/plain; charset=utf8',[m
[32m+[m[32m       '',[m
[32m+[m[32m       'ab',[m
[32m+[m[32m       '-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m       'Content-Disposition: form-data; '[m
[32m+[m[32m         + 'name="upload_file_1"; filename="notes2.txt"',[m
[32m+[m[32m       'Content-Type: text/plain; charset=utf8',[m
[32m+[m[32m       '',[m
[32m+[m[32m       'cd',[m
[32m+[m[32m       '-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k--',[m
[32m+[m[32m      ].join('\r\n')[m
[32m+[m[32m    ],[m
[32m+[m[32m    boundary: '---------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m    limits: {[m
[32m+[m[32m      files: 1,[m
[32m+[m[32m    },[m
[32m+[m[32m    expected: [[m
[32m+[m[32m      { type: 'file',[m
[32m+[m[32m        name: 'upload_file_0',[m
[32m+[m[32m        data: Buffer.from('ab'),[m
[32m+[m[32m        info: {[m
[32m+[m[32m          filename: 'notes.txt',[m
[32m+[m[32m          encoding: '7bit',[m
[32m+[m[32m          mimeType: 'text/plain',[m
[32m+[m[32m        },[m
[32m+[m[32m        limited: false,[m
[32m+[m[32m      },[m
[32m+[m[32m      'filesLimit',[m
[32m+[m[32m    ],[m
[32m+[m[32m    what: 'Files limit'[m
[32m+[m[32m  },[m
[32m+[m[32m  { source: [[m
[32m+[m[32m      ['-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m       'Content-Disposition: form-data; '[m
[32m+[m[32m         + `name="upload_file_0"; filename="${'a'.repeat(64 * 1024)}.txt"`,[m
[32m+[m[32m       'Content-Type: text/plain; charset=utf8',[m
[32m+[m[32m       '',[m
[32m+[m[32m       'ab',[m
[32m+[m[32m       '-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m       'Content-Disposition: form-data; '[m
[32m+[m[32m         + 'name="upload_file_1"; filename="notes2.txt"',[m
[32m+[m[32m       'Content-Type: text/plain; charset=utf8',[m
[32m+[m[32m       '',[m
[32m+[m[32m       'cd',[m
[32m+[m[32m       '-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k--',[m
[32m+[m[32m      ].join('\r\n')[m
[32m+[m[32m    ],[m
[32m+[m[32m    boundary: '---------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m    expected: [[m
[32m+[m[32m      { error: 'Malformed part header' },[m
[32m+[m[32m      { type: 'file',[m
[32m+[m[32m        name: 'upload_file_1',[m
[32m+[m[32m        data: Buffer.from('cd'),[m
[32m+[m[32m        info: {[m
[32m+[m[32m          filename: 'notes2.txt',[m
[32m+[m[32m          encoding: '7bit',[m
[32m+[m[32m          mimeType: 'text/plain',[m
[32m+[m[32m        },[m
[32m+[m[32m        limited: false,[m
[32m+[m[32m      },[m
[32m+[m[32m    ],[m
[32m+[m[32m    what: 'Oversized part header'[m
[32m+[m[32m  },[m
[32m+[m[32m  { source: [[m
[32m+[m[32m      ['-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m       'Content-Disposition: form-data; '[m
[32m+[m[32m         + 'name="upload_file_0"; filename="notes.txt"',[m
[32m+[m[32m       'Content-Type: text/plain; charset=utf8',[m
[32m+[m[32m       '',[m
[32m+[m[32m       'a'.repeat(31) + '\r',[m
[32m+[m[32m      ].join('\r\n'),[m
[32m+[m[32m      'b'.repeat(40),[m
[32m+[m[32m      '\r\n-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k--',[m
[32m+[m[32m    ],[m
[32m+[m[32m    boundary: '---------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m    fileHwm: 32,[m
[32m+[m[32m    expected: [[m
[32m+[m[32m      { type: 'file',[m
[32m+[m[32m        name: 'upload_file_0',[m
[32m+[m[32m        data: Buffer.from('a'.repeat(31) + '\r' + 'b'.repeat(40)),[m
[32m+[m[32m        info: {[m
[32m+[m[32m          filename: 'notes.txt',[m
[32m+[m[32m          encoding: '7bit',[m
[32m+[m[32m          mimeType: 'text/plain',[m
[32m+[m[32m        },[m
[32m+[m[32m        limited: false,[m
[32m+[m[32m      },[m
[32m+[m[32m    ],[m
[32m+[m[32m    what: 'Lookbehind data should not stall file streams'[m
[32m+[m[32m  },[m
[32m+[m[32m  { source: [[m
[32m+[m[32m      ['-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m       'Content-Disposition: form-data; '[m
[32m+[m[32m         + `name="upload_file_0"; filename="${'a'.repeat(8 * 1024)}.txt"`,[m
[32m+[m[32m       'Content-Type: text/plain; charset=utf8',[m
[32m+[m[32m       '',[m
[32m+[m[32m       'ab',[m
[32m+[m[32m       '-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m       'Content-Disposition: form-data; '[m
[32m+[m[32m         + `name="upload_file_1"; filename="${'b'.repeat(8 * 1024)}.txt"`,[m
[32m+[m[32m       'Content-Type: text/plain; charset=utf8',[m
[32m+[m[32m       '',[m
[32m+[m[32m       'cd',[m
[32m+[m[32m       '-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m       'Content-Disposition: form-data; '[m
[32m+[m[32m         + `name="upload_file_2"; filename="${'c'.repeat(8 * 1024)}.txt"`,[m
[32m+[m[32m       'Content-Type: text/plain; charset=utf8',[m
[32m+[m[32m       '',[m
[32m+[m[32m       'ef',[m
[32m+[m[32m       '-----------------------------paZqsnEHRufoShdX6fh0lUhXBP4k--',[m
[32m+[m[32m      ].join('\r\n')[m
[32m+[m[32m    ],[m
[32m+[m[32m    boundary: '---------------------------paZqsnEHRufoShdX6fh0lUhXBP4k',[m
[32m+[m[32m    expected: [[m
[32m+[m[32m      { type: 'file',[m
[32m+[m[32m        name: 'upload_file_0',[m
[32m+[m[32m        data: Buffer.from('ab'),[m
[32m+[m[32m        info: {[m
[32m+[m[32m          filename: `${'a'.repeat(8 * 1024)}.txt`,[m
[32m+[m[32m          encoding: '7bit',[m
[32m+[m[32m          mimeType: 'text/plain',[m
[32m+[m[32m        },[m
[32m+[m[32m        limited: false,[m
[32m+[m[32m      },[m
[32m+[m[32m      { type: 'file',[m
[32m+[m[32m        name: 'upload_file_1',[m
[32m+[m[32m        data: Buffer.from('cd'),[m
[32m+[m[32m        info: {[m
[32m+[m[32m          filename: `${'b'.repeat(8 * 1024)}.txt`,[m
[32m+[m[32m          encoding: '7bit',[m
[32m+[m[32m          mimeType: 'text/plain',[m
[32m+[m[32m        },[m
[32m+[m[32m        limited: false,[m
[32m+[m[32m      },[m
[32m+[m[32m      { type: 'file',[m
[32m+[m[32m        name: 'upload_file_2',[m
[32m+[m[32m        data: Buffer.from('ef'),[m
[32m+[m[32m        info: {[m
[32m+[m[32m          filename: `${'c'.repeat(8 * 1024)}.txt`,[m
[32m+[m[32m          encoding: '7bit',[m
[32m+[m[32m          mimeType: 'text/plain',[m
[32m+[m[32m        },[m
[32m+[m[32m        limited: false,[m
[32m+[m[32m      },[m
[32m+[m[32m    ],[m
[32m+[m[32m    what: 'Header size limit should be per part'[m
[32m+[m[32m  },[m
[32m+[m[32m  { source: [[m
[32m+[m[32m      '\r\n--d1bf46b3-aa33-4061-b28d-6c5ced8b08ee\r\n',[m
[32m+[m[32m      'Content-Type: application/gzip\r\n'[m
[32m+[m[32m        + 'Content-Encoding: gzip\r\n'[m
[32m+[m[32m        + 'Content-Disposition: form-data; name=batch-1; filename=batch-1'[m
[32m+[m[32m        + '\r\n\r\n',[m
[32m+[m[32m      '\r\n--d1bf46b3-aa33-4061-b28d-6c5ced8b08ee--',[m
[32m+[m[32m    ],[m
[32m+[m[32m    boundary: 'd1bf46b3-aa33-4061-b28d-6c5ced8b08ee',[m
[32m+[m[32m    expected: [[m
[32m+[m[32m      { type: 'file',[m
[32m+[m[32m        name: 'batch-1',[m
[32m+[m[32m        data: Buffer.alloc(0),[m
[32m+[m[32m        info: {[m
[32m+[m[32m          filename: 'batch-1',[m
[32m+[m[32m          encoding: '7bit',[m
[32m+[m[32m          mimeType: 'application/gzip',[m
[32m+[m[32m        },[m
[32m+[m[32m        limited: false,[m
[32m+[m[32m      },[m
[32m+[m[32m    ],[m
[32m+[m[32m    what: 'Empty part'[m
[32m+[m[32m  },[m
[32m+[m[32m];[m
[32m+[m
[32m+[m[32mfor (const test of tests) {[m
[32m+[m[32m  active.set(test, 1);[m
[32m+[m
[32m+[m[32m  const { what, boundary, events, limits, preservePath, fileHwm } = test;[m
[32m+[m[32m  const bb = busboy({[m
[32m+[m[32m    fileHwm,[m
[32m+[m[32m    limits,[m
[32m+[m[32m    preservePath,[m
[32m+[m[32m    headers: {[m
[32m+[m[32m      'content-type': `multipart/form-data; boundary=${boundary}`,[m
[32m+[m[32m    }[m
[32m+[m[32m  });[m
[32m+[m[32m  const results = [];[m
[32m+[m
[32m+[m[32m  if (events === undefined || events.includes('field')) {[m
[32m+[m[32m    bb.on('field', (name, val, info) => {[m
[32m+[m[32m      results.push({ type: 'field', name, val, info });[m
[32m+[m[32m    });[m
[32m+[m[32m  }[m
[32m+[m
[32m+[m[32m  if (events === undefined || events.includes('file')) {[m
[32m+[m[32m    bb.on('file', (name, stream, info) => {[m
[32m+[m[32m      const data = [];[m
[32m+[m[32m      let nb = 0;[m
[32m+[m[32m      const file = {[m
[32m+[m[32m        type: 'file',[m
[32m+[m[32m        name,[m
[32m+[m[32m        data: null,[m
[32m+[m[32m        info,[m
[32m+[m[32m        limited: false,[m
[32m+[m[32m      };[m
[32m+[m[32m      results.push(file);[m
[32m+[m[32m      stream.on('data', (d) => {[m
[32m+[m[32m        data.push(d);[m
[32m+[m[32m        nb += d.length;[m
[32m+[m[32m      }).on('limit', () => {[m
[32m+[m[32m        file.limited = true;[m
[32m+[m[32m      }).on('close', () => {[m
[32m+[m[32m        file.data = Buffer.concat(data, nb);[m
[32m+[m[32m        assert.strictEqual(stream.truncated, file.limited);[m
[32m+[m[32m      }).once('error', (err) => {[m
[32m+[m[32m        file.err = err.message;[m
[32m+[m[32m      });[m
[32m+[m[32m    });[m
[32m+[m[32m  }[m
[32m+[m
[32m+[m[32m  bb.on('error', (err) => {[m
[32m+[m[32m    results.push({ error: err.message });[m
[32m+[m[32m  });[m
[32m+[m
[32m+[m[32m  bb.on('partsLimit', () => {[m
[32m+[m[32m    results.push('partsLimit');[m
[32m+[m[32m  });[m
[32m+[m
[32m+[m[32m  bb.on('filesLimit', () => {[m
[32m+[m[32m    results.push('filesLimit');[m
[32m+[m[32m  });[m
[32m+[m
[32m+[m[32m  bb.on('fieldsLimit', () => {[m
[32m+[m[32m    results.push('fieldsLimit');[m
[32m+[m[32m  });[m
[32m+[m
[32m+[m[32m  bb.on('close', () => {[m
[32m+[m[32m    active.delete(test);[m
[32m+[m
[32m+[m[32m    assert.deepStrictEqual([m
[32m+[m[32m      results,[m
[32m+[m[32m      test.expected,[m
[32m+[m[32m      `[${what}] Results mismatch.\n`[m
[32m+[m[32m        + `Parsed: ${inspect(results)}\n`[m
[32m+[m[32m        + `Expected: ${inspect(test.expected)}`[m
[32m+[m[32m    );[m
[32m+[m[32m  });[m
[32m+[m
[32m+[m[32m  for (const src of test.source) {[m
[32m+[m[32m    const buf = (typeof src === 'string' ? Buffer.from(src, 'utf8') : src);[m
[32m+[m[32m    bb.write(buf);[m
[32m+[m[32m  }[m
[32m+[m[32m  bb.end();[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32m// Byte-by-byte versions[m
[32m+[m[32mfor (let test of tests) {[m
[32m+[m[32m  test = { ...test };[m
[32m+[m[32m  test.what += ' (byte-by-byte)';[m
[32m+[m[32m  active.set(test, 1);[m
[32m+[m
[32m+[m[32m  const { what, boundary, events, limits, preservePath, fileHwm } = test;[m
[32m+[m[32m  const bb = busboy({[m
[32m+[m[32m    fileHwm,[m
[32m+[m[32m    limits,[m
[32m+[m[32m    preservePath,[m
[32m+[m[32m    headers: {[m
[32m+[m[32m      'content-type': `multipart/form-data; boundary=${boundary}`,[m
[32m+[m[32m    }[m
[32m+[m[32m  });[m
[32m+[m[32m  const results = [];[m
[32m+[m
[32m+[m[32m  if (events === undefined || events.includes('field')) {[m
[32m+[m[32m    bb.on('field', (name, val, info) => {[m
[32m+[m[32m      results.push({ type: 'field', name, val, info });[m
[32m+[m[32m    });[m
[32m+[m[32m  }[m
[32m+[m
[32m+[m[32m  if (events === undefined || events.includes('file')) {[m
[32m+[m[32m    bb.on('file', (name, stream, info) => {[m
[32m+[m[32m      const data = [];[m
[32m+[m[32m      let nb = 0;[m
[32m+[m[32m      const file = {[m
[32m+[m[32m        type: 'file',[m
[32m+[m[32m        name,[m
[32m+[m[32m        data: null,[m
[32m+[m[32m        info,[m
[32m+[m[32m        limited: false,[m
[32m+[m[32m      };[m
[32m+[m[32m      results.push(file);[m
[32m+[m[32m      stream.on('data', (d) => {[m
[32m+[m[32m        data.push(d);[m
[32m+[m[32m        nb += d.length;[m
[32m+[m[32m      }).on('limit', () => {[m
[32m+[m[32m        file.limited = true;[m
[32m+[m[32m      }).on('close', () => {[m
[32m+[m[32m        file.data = Buffer.concat(data, nb);[m
[32m+[m[32m        assert.strictEqual(stream.truncated, file.limited);[m
[32m+[m[32m      }).once('error', (err) => {[m
[32m+[m[32m        file.err = err.message;[m
[32m+[m[32m      });[m
[32m+[m[32m    });[m
[32m+[m[32m  }[m
[32m+[m
[32m+[m[32m  bb.on('error', (err) => {[m
[32m+[m[32m    results.push({ error: err.message });[m
[32m+[m[32m  });[m
[32m+[m
[32m+[m[32m  bb.on('partsLimit', () => {[m
[32m+[m[32m    results.push('partsLimit');[m
[32m+[m[32m  });[m
[32m+[m
[32m+[m[32m  bb.on('filesLimit', () => {[m
[32m+[m[32m    results.push('filesLimit');[m
[32m+[m[32m  });[m
[32m+[m
[32m+[m[32m  bb.on('fieldsLimit', () => {[m
[32m+[m[32m    results.push('fieldsLimit');[m
[32m+[m[32m  });[m
[32m+[m
[32m+[m[32m  bb.on('close', () => {[m
[32m+[m[32m    active.delete(test);[m
[32m+[m
[32m+[m[32m    assert.deepStrictEqual([m
[32m+[m[32m      results,[m
[32m+[m[32m      test.expected,[m
[32m+[m[32m      `[${what}] Results mismatch.\n`[m
[32m+[m[32m        + `Parsed: ${inspect(results)}\n`[m
[32m+[m[32m        + `Expected: ${inspect(test.expected)}`[m
[32m+[m[32m    );[m
[32m+[m[32m  });[m
[32m+[m
[32m+[m[32m  for (const src of test.source) {[m
[32m+[m[32m    const buf = (typeof src === 'string' ? Buffer.from(src, 'utf8') : src);[m
[32m+[m[32m    for (let i = 0; i < buf.length; ++i)[m
[32m+[m[32m      bb.write(buf.slice(i, i + 1));[m
[32m+[m[32m  }[m
[32m+[m[32m  bb.end();[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32m{[m
[32m+[m[32m  let exception = false;[m
[32m+[m[32m  process.once('uncaughtException', (ex) => {[m
[32m+[m[32m    exception = true;[m
[32m+[m[32m    throw ex;[m
[32m+[m[32m  });[m
[32m+[m[32m  process.on('exit', () => {[m
[32m+[m[32m    if (exception || active.size === 0)[m
[32m+[m[32m      return;[m
[32m+[m[32m    process.exitCode = 1;[m
[32m+[m[32m    console.error('==========================');[m
[32m+[m[32m    console.error(`${active.size} test(s) did not finish:`);[m
[32m+[m[32m    console.error('==========================');[m
[32m+[m[32m    console.error(Array.from(active.keys()).map((v) => v.what).join('\n'));[m
[32m+[m[32m  });[m
[32m+[m[32m}[m
[1mdiff --git a/node_modules/busboy/test/test-types-urlencoded.js b/node_modules/busboy/test/test-types-urlencoded.js[m
[1mnew file mode 100644[m
[1mindex 0000000..c35962b[m
[1m--- /dev/null[m
[1m+++ b/node_modules/busboy/test/test-types-urlencoded.js[m
[36m@@ -0,0 +1,488 @@[m
[32m+[m[32m'use strict';[m
[32m+[m
[32m+[m[32mconst assert = require('assert');[m
[32m+[m[32mconst { transcode } = require('buffer');[m
[32m+[m[32mconst { inspect } = require('util');[m
[32m+[m
[32m+[m[32mconst busboy = require('..');[m
[32m+[m
[32m+[m[32mconst active = new Map();[m
[32m+[m
[32m+[m[32mconst tests = [[m
[32m+[m[32m  { source: ['foo'],[m
[32m+[m[32m    expected: [[m
[32m+[m[32m      ['foo',[m
[32m+[m[32m       '',[m
[32m+[m[32m       { nameTruncated: false,[m
[32m+[m[32m         valueTruncated: false,[m
[32m+[m[32m         encoding: 'utf-8',[m
[32m+[m[32m         mimeType: 'text/plain' },[m
[32m+[m[32m      ],[m
[32m+[m[32m    ],[m
[32m+[m[32m    what: 'Unassigned value'[m
[32m+[m[32m  },[m
[32m+[m[32m  { source: ['foo=bar'],[m
[32m+[m[32m    expected: [[m
[32m+[m[32m      ['foo',[m
[32m+[m[32m       'bar',[m
[32m+[m[32m       { nameTruncated: false,[m
[32m+[m[32m         valueTruncated: false,[m
[32m+[m[32m         encoding: 'utf-8',[m
[32m+[m[32m         mimeType: 'text/plain' },[m
[32m+[m[32m      ],[m
[32m+[m[32m    ],[m
[32m+[m[32m    what: 'Assigned value'[m
[32m+[m[32m  },[m
[32m+[m[32m  { source: ['foo&bar=baz'],[m
[32m+[m[32m    expected: [[m
[32m+[m[32m      ['foo',[m
[32m+[m[32m       '',[m
[32m+[m[32m       { nameTruncated: false,[m
[32m+[m[32m         valueTruncated: false,[m
[32m+[m[32m         encoding: 'utf-8',[m
[32m+[m[32m         mimeType: 'text/plain' },[m
[32m+[m[32m      ],[m
[32m+[m[32m      ['bar',[m
[32m+[m[32m       'baz',[m
[32m+[m[32m       { nameTruncated: false,[m
[32m+[m[32m         valueTruncated: false,[m
[32m+[m[32m         encoding: 'utf-8',[m
[32m+[m[32m         mimeType: 'text/plain' },[m
[32m+[m[32m      ],[m
[32m+[m[32m    ],[m
[32m+[m[32m    what: 'Unassigned and assigned value'[m
[32m+[m[32m  },[m
[32m+[m[32m  { source: ['foo=bar&baz'],[m
[32m+[m[32m    expected: [[m
[32m+[m[32m      ['foo',[m
[32m+[m[32m       'bar',[m
[32m+[m[32m       { nameTruncated: false,[m
[32m+[m[32m         valueTruncated: false,[m
[32m+[m[32m         encoding: 'utf-8',[m
[32m+[m[32m         mimeType: 'text/plain' },[m
[32m+[m[32m      ],[m
[32m+[m[32m      ['baz',[m
[32m+[m[32m       '',[m
[32m+[m[32m       { nameTruncated: false,[m
[32m+[m[32m         valueTruncated: false,[m
[32m+[m[32m         encoding: 'utf-8',[m
[32m+[m[32m         mimeType: 'text/plain' },[m
[32m+[m[32m      ],[m
[32m+[m[32m    ],[m
[32m+[m[32m    what: 'Assigned and unassigned value'[m
[32m+[m[32m  },[m
[32m+[m[32m  { source: ['foo=bar&baz=bla'],[m
[32m+[m[32m    expected: [[m
[32m+[m[32m      ['foo',[m
[32m+[m[32m       'bar',[m
[32m+[m[32m       { nameTruncated: false,[m
[32m+[m[32m         valueTruncated: false,[m
[32m+[m[32m         encoding: 'utf-8',[m
[32m+[m[32m         mimeType: 'text/plain' },[m
[32m+[m[32m      ],[m
[32m+[m[32m      ['baz',[m
[32m+[m[32m       'bla',[m
[32m+[m[32m       { nameTruncated: false,[m
[32m+[m[32m         valueTruncated: false,[m
[32m+[m[32m         encoding: 'utf-8',[m
[32m+[m[32m         mimeType: 'text/plain' },[m
[32m+[m[32m      ],[m
[32m+[m[32m    ],[m
[32m+[m[32m    what: 'Two assigned values'[m
[32m+[m[32m  },[m
[32m+[m[32m  { source: ['foo&bar'],[m
[32m+[m[32m    expected: [[m
[32m+[m[32m      ['foo',[m
[32m+[m[32m       '',[m
[32m+[m[32m       { nameTruncated: false,[m
[32m+[m[32m         valueTruncated: false,[m
[32m+[m[32m         encoding: 'utf-8',[m
[32m+[m[32m         mimeType: 'text/plain' },[m
[32m+[m[32m      ],[m
[32m+[m[32m      ['bar',[m
[32m+[m[32m       '',[m
[32m+[m[32m       { nameTruncated: false,[m
[32m+[m[32m         valueTruncated: false,[m
[32m+[m[32m         encoding: 'utf-8',[m
[32m+[m[32m         mimeType: 'text/plain' },[m
[32m+[m[32m      ],[m
[32m+[m[32m    ],[m
[32m+[m[32m    what: 'Two unassigned values'[m
[32m+[m[32m  },[m
[32m+[m[32m  { source: ['foo&bar&'],[m
[32m+[m[32m    expected: [[m
[32m+[m[32m      ['foo',[m
[32m+[m[32m       '',[m
[32m+[m[32m       { nameTruncated: false,[m
[32m+[m[32m         valueTruncated: false,[m
[32m+[m[32m         encoding: 'utf-8',[m
[32m+[m[32m         mimeType: 'text/plain' },[m
[32m+[m[32m      ],[m
[32m+[m[32m      ['bar',[m
[32m+[m[32m       '',[m
[32m+[m[32m       { nameTruncated: false,[m
[32m+[m[32m         valueTruncated: false,[m
[32m+[m[32m         encoding: 'utf-8',[m
[32m+[m[32m         mimeType: 'text/plain' },[m
[32m+[m[32m      ],[m
[32m+[m[32m    ],[m
[32m+[m[32m    what: 'Two unassigned values and ampersand'[m
[32m+[m[32m  },[m
[32m+[m[32m  { source: ['foo+1=bar+baz%2Bquux'],[m
[32m+[m[32m    expected: [[m
[32m+[m[32m      ['foo 1',[m
[32m+[m[32m       'bar baz+quux',[m
[32m+[m[32m       { nameTruncated: false,[m
[32m+[m[32m         valueTruncated: false,[m
[32m+[m[32m         encoding: 'utf-8',[m
[32m+[m[32m         mimeType: 'text/plain' },[m
[32m+[m[32m      ],[m
[32m+[m[32m    ],[m
[32m+[m[32m    what: 'Assigned key and value with (plus) space'[m
[32m+[m[32m  },[m
[32m+[m[32m  { source: ['foo=bar%20baz%21'],[m
[32m+[m[32m    expected: [[m
[32m+[m[32m      ['foo',[m
[32m+[m[32m       'bar baz!',[m
[32m+[m[32m       { nameTruncated: false,[m
[32m+[m[32m         valueTruncated: false,[m
[32m+[m[32m         encoding: 'utf-8',[m
[32m+[m[32m         mimeType: 'text/plain' },[m
[32m+[m[32m      ],[m
[32m+[m[32m    ],[m
[32m+[m[32m    what: 'Assigned value with encoded bytes'[m
[32m+[m[32m  },[m
[32m+[m[32m  { source: ['foo%20bar=baz%20bla%21'],[m
[32m+[m[32m    expected: [[m
[32m+[m[32m      ['foo bar',[m
[32m+[m[32m       'baz bla!',[m
[32m+[m[32m       { nameTruncated: false,[m
[32m+[m[32m         valueTruncated: false,[m
[32m+[m[32m         encoding: 'utf-8',[m
[32m+[m[32m         mimeType: 'text/plain' },[m
[32m+[m[32m      ],[m
[32m+[m[32m    ],[m
[32m+[m[32m    what: 'Assigned value with encoded bytes #2'[m
[32m+[m[32m  },[m
[32m+[m[32m  { source: ['foo=bar%20baz%21&num=1000'],[m
[32m+[m[32m    expected: [[m
[32m+[m[32m      ['foo',[m
[32m+[m[32m       'bar baz!',[m
[32m+[m[32m       { nameTruncated: false,[m
[32m+[m[32m         valueTruncated: false,[m
[32m+[m[32m         encoding: 'utf-8',[m
[32m+[m[32m         mimeType: 'text/plain' },[m
[32m+[m[32m      ],[m
[32m+[m[32m      ['num',[m
[32m+[m[32m       '1000',[m
[32m+[m[32m       { nameTruncated: false,[m
[32m+[m[32m         valueTruncated: false,[m
[32m+[m[32m         encoding: 'utf-8',[m
[32m+[m[32m         mimeType: 'text/plain' },[m
[32m+[m[32m      ],[m
[32m+[m[32m    ],[m
[32m+[m[32m    what: 'Two assigned values, one with encoded bytes'[m
[32m+[m[32m  },[m
[32m+[m[32m  { source: [[m
[32m+[m[32m      Array.from(transcode(Buffer.from('foo'), 'utf8', 'utf16le')).map([m
[32m+[m[32m        (n) => `%${n.toString(16).padStart(2, '0')}`[m
[32m+[m[32m      ).join(''),[m
[32m+[m[32m      '=',[m
[32m+[m[32m      Array.from(transcode(Buffer.from('😀!'), 'utf8', 'utf16le')).map([m
[32m+[m[32m        (n) => `%${n.toString(16).padStart(2, '0')}`[m
[32m+[m[32m      ).join(''),[m
[32m+[m[32m    ],[m
[32m+[m[32m    expected: [[m
[32m+[m[32m      ['foo',[m
[32m+[m[32m       '😀!',[m
[32m+[m[32m       { nameTruncated: false,[m
[32m+[m[32m         valueTruncated: false,[m
[32m+[m[32m         encoding: 'UTF-16LE',[m
[32m+[m[32m         mimeType: 'text/plain' },[m
[32m+[m[32m      ],[m
[32m+[m[32m    ],[m
[32m+[m[32m    charset: 'UTF-16LE',[m
[32m+[m[32m    what: 'Encoded value with multi-byte charset'[m
[32m+[m[32m  },[m
[32m+[m[32m  { source: [[m
[32m+[m[32m      'foo=<',[m
[32m+[m[32m      Array.from(transcode(Buffer.from('©:^þ'), 'utf8', 'latin1')).map([m
[32m+[m[32m        (n) => `%${n.toString(16).padStart(2, '0')}`[m
[32m+[m[32m      ).join(''),[m
[32m+[m[32m    ],[m
[32m+[m[32m    expected: [[m
[32m+[m[32m      ['foo',[m
[32m+[m[32m       '<©:^þ',[m
[32m+[m[32m       { nameTruncated: false,[m
[32m+[m[32m         valueTruncated: false,[m
[32m+[m[32m         encoding: 'ISO-8859-1',[m
[32m+[m[32m         mimeType: 'text/plain' },[m
[32m+[m[32m      ],[m
[32m+[m[32m    ],[m
[32m+[m[32m    charset: 'ISO-8859-1',[m
[32m+[m[32m    what: 'Encoded value with single-byte, ASCII-compatible, non-UTF8 charset'[m
[32m+[m[32m  },[m
[32m+[m[32m  { source: ['foo=bar&baz=bla'],[m
[32m+[m[32m    expected: [],[m
[32m+[m[32m    what: 'Limits: zero fields',[m
[32m+[m[32m    limits: { fields: 0 }[m
[32m+[m[32m  },[m
[32m+[m[32m  { source: ['foo=bar&baz=bla'],[m
[32m+[m[32m    expected: [[m
[32m+[m[32m      ['foo',[m
[32m+[m[32m       'bar',[m
[32m+[m[32m       { nameTruncated: false,[m
[32m+[m[32m         valueTruncated: false,[m
[32m+[m[32m         encoding: 'utf-8',[m
[32m+[m[32m         mimeType: 'text/plain' },[m
[32m+[m[32m      ],[m
[32m+[m[32m    ],[m
[32m+[m[32m    what: 'Limits: one field',[m
[32m+[m[32m    limits: { fields: 1 }[m
[32m+[m[32m  },[m
[32m+[m[32m  { source: ['foo=bar&baz=bla'],[m
[32m+[m[32m    expected: [[m
[32m+[m[32m      ['foo',[m
[32m+[m[32m       'bar',[m
[32m+[m[32m       { nameTruncated: false,[m
[32m+[m[32m         valueTruncated: false,[m
[32m+[m[32m         encoding: 'utf-8',[m
[32m+[m[32m         mimeType: 'text/plain' },[m
[32m+[m[32m      ],[m
[32m+[m[32m      ['baz',[m
[32m+[m[32m       'bla',[m
[32m+[m[32m       { nameTruncated: false,[m
[32m+[m[32m         valueTruncated: false,[m
[32m+[m[32m         encoding: 'utf-8',[m
[32m+[m[32m         mimeType: 'text/plain' },[m
[32m+[m[32m      ],[m
[32m+[m[32m    ],[m
[32m+[m[32m    what: 'Limits: field part lengths match limits',[m
[32m+[m[32m    limits: { fieldNameSize: 3, fieldSize: 3 }[m
[32m+[m[32m  },[m
[32m+[m[32m  { source: ['foo=bar&baz=bla'],[m
[32m+[m[32m    expected: [[m
[32m+[m[32m      ['fo',[m
[32m+[m[32m       'bar',[m
[32m+[m[32m       { nameTruncated: true,[m
[32m+[m[32m         valueTruncated: false,[m
[32m+[m[32m         encoding: 'utf-8',[m
[32m+[m[32m         mimeType: 'text/plain' },[m
[32m+[m[32m      ],[m
[32m+[m[32m      ['ba',[m
[32m+[m[32m       'bla',[m
[32m+[m[32m       { nameTruncated: true,[m
[32m+[m[32m         valueTruncated: false,[m
[32m+[m[32m         encoding: 'utf-8',[m
[32m+[m[32m         mimeType: 'text/plain' },[m
[32m+[m[32m      ],[m
[32m+[m[32m    ],[m
[32m+[m[32m    what: 'Limits: truncated field name',[m
[32m+[m[32m    limits: { fieldNameSize: 2 }[m
[32m+[m[32m  },[m
[32m+[m[32m  { source: ['foo=bar&baz=bla'],[m
[32m+[m[32m    expected: [[m
[32m+[m[32m      ['foo',[m
[32m+[m[32m       'ba',[m
[32m+[m[32m       { nameTruncated: false,[m
[32m+[m[32m         valueTruncated: true,[m
[32m+[m[32m         encoding: 'utf-8',[m
[32m+[m[32m         mimeType: 'text/plain' },[m
[32m+[m[32m      ],[m
[32m+[m[32m      ['baz',[m
[32m+[m[32m       'bl',[m
[32m+[m[32m       { nameTruncated: false,[m
[32m+[m[32m         valueTruncated: true,[m
[32m+[m[32m         encoding: 'utf-8',[m
[32m+[m[32m         mimeType: 'text/plain' },[m
[32m+[m[32m      ],[m
[32m+[m[32m    ],[m
[32m+[m[32m    what: 'Limits: truncated field value',[m
[32m+[m[32m    limits: { fieldSize: 2 }[m
[32m+[m[32m  },[m
[32m+[m[32m  { source: ['foo=bar&baz=bla'],[m
[32m+[m[32m    expected: [[m
[32m+[m[32m      ['fo',[m
[32m+[m[32m       'ba',[m
[32m+[m[32m       { nameTruncated: true,[m
[32m+[m[32m         valueTruncated: true,[m
[32m+[m[32m         encoding: 'utf-8',[m
[32m+[m[32m         mimeType: 'text/plain' },[m
[32m+[m[32m      ],[m
[32m+[m[32m      ['ba',[m
[32m+[m[32m       'bl',[m
[32m+[m[32m       { nameTruncated: true,[m
[32m+[m[32m         valueTruncated: true,[m
[32m+[m[32m         encoding: 'utf-8',[m
[32m+[m[32m         mimeType: 'text/plain' },[m
[32m+[m[32m      ],[m
[32m+[m[32m    ],[m
[32m+[m[32m    what: 'Limits: truncated field name and value',[m
[32m+[m[32m    limits: { fieldNameSize: 2, fieldSize: 2 }[m
[32m+[m[32m  },[m
[32m+[m[32m  { source: ['foo=bar&baz=bla'],[m
[32m+[m[32m    expected: [[m
[32m+[m[32m      ['fo',[m
[32m+[m[32m       '',[m
[32m+[m[32m       { nameTruncated: true,[m
[32m+[m[32m         valueTruncated: true,[m
[32m+[m[32m         encoding: 'utf-8',[m
[32m+[m[32m         mimeType: 'text/plain' },[m
[32m+[m[32m      ],[m
[32m+[m[32m      ['ba',[m
[32m+[m[32m       '',[m
[32m+[m[32m       { nameTruncated: true,[m
[32m+[m[32m         valueTruncated: true,[m
[32m+[m[32m         encoding: 'utf-8',[m
[32m+[m[32m         mimeType: 'text/plain' },[m
[32m+[m[32m      ],[m
[32m+[m[32m    ],[m
[32m+[m[32m    what: 'Limits: truncated field name and zero value limit',[m
[32m+[m[32m    limits: { fieldNameSize: 2, fieldSize: 0 }[m
[32m+[m[32m  },[m
[32m+[m[32m  { source: ['foo=bar&baz=bla'],[m
[32m+[m[32m    expected: [[m
[32m+[m[32m      ['',[m
[32m+[m[32m       '',[m
[32m+[m[32m       { nameTruncated: true,[m
[32m+[m[32m         valueTruncated: true,[m
[32m+[m[32m         encoding: 'utf-8',[m
[32m+[m[32m         mimeType: 'text/plain' },[m
[32m+[m[32m      ],[m
[32m+[m[32m      ['',[m
[32m+[m[32m       '',[m
[32m+[m[32m       { nameTruncated: true,[m
[32m+[m[32m         valueTruncated: true,[m
[32m+[m[32m         encoding: 'utf-8',[m
[32m+[m[32m         mimeType: 'text/plain' },[m
[32m+[m[32m      ],[m
[32m+[m[32m    ],[m
[32m+[m[32m    what: 'Limits: truncated zero field name and zero value limit',[m
[32m+[m[32m    limits: { fieldNameSize: 0, fieldSize: 0 }[m
[32m+[m[32m  },[m
[32m+[m[32m  { source: ['&'],[m
[32m+[m[32m    expected: [],[m
[32m+[m[32m    what: 'Ampersand'[m
[32m+[m[32m  },[m
[32m+[m[32m  { source: ['&&&&&'],[m
[32m+[m[32m    expected: [],[m
[32m+[m[32m    what: 'Many ampersands'[m
[32m+[m[32m  },[m
[32m+[m[32m  { source: ['='],[m
[32m+[m[32m    expected: [[m
[32m+[m[32m      ['',[m
[32m+[m[32m       '',[m
[32m+[m[32m       { nameTruncated: false,[m
[32m+[m[32m         valueTruncated: false,[m
[32m+[m[32m         encoding: 'utf-8',[m
[32m+[m[32m         mimeType: 'text/plain' },[m
[32m+[m[32m      ],[m
[32m+[m[32m    ],[m
[32m+[m[32m    what: 'Assigned value, empty name and value'[m
[32m+[m[32m  },[m
[32m+[m[32m  { source: [''],[m
[32m+[m[32m    expected: [],[m
[32m+[m[32m    what: 'Nothing'[m
[32m+[m[32m  },[m
[32m+[m[32m];[m
[32m+[m
[32m+[m[32mfor (const test of tests) {[m
[32m+[m[32m  active.set(test, 1);[m
[32m+[m
[32m+[m[32m  const { what } = test;[m
[32m+[m[32m  const charset = test.charset || 'utf-8';[m
[32m+[m[32m  const bb = busboy({[m
[32m+[m[32m    limits: test.limits,[m
[32m+[m[32m    headers: {[m
[32m+[m[32m      'content-type': `application/x-www-form-urlencoded; charset=${charset}`,[m
[32m+[m[32m    },[m
[32m+[m[32m  });[m
[32m+[m[32m  const results = [];[m
[32m+[m
[32m+[m[32m  bb.on('field', (key, val, info) => {[m
[32m+[m[32m    results.push([key, val, info]);[m
[32m+[m[32m  });[m
[32m+[m
[32m+[m[32m  bb.on('file', () => {[m
[32m+[m[32m    throw new Error(`[${what}] Unexpected file`);[m
[32m+[m[32m  });[m
[32m+[m
[32m+[m[32m  bb.on('close', () => {[m
[32m+[m[32m    active.delete(test);[m
[32m+[m
[32m+[m[32m    assert.deepStrictEqual([m
[32m+[m[32m      results,[m
[32m+[m[32m      test.expected,[m
[32m+[m[32m      `[${what}] Results mismatch.\n`[m
[32m+[m[32m        + `Parsed: ${inspect(results)}\n`[m
[32m+[m[32m        + `Expected: ${inspect(test.expected)}`[m
[32m+[m[32m    );[m
[32m+[m[32m  });[m
[32m+[m
[32m+[m[32m  for (const src of test.source) {[m
[32m+[m[32m    const buf = (typeof src === 'string' ? Buffer.from(src, 'utf8') : src);[m
[32m+[m[32m    bb.write(buf);[m
[32m+[m[32m  }[m
[32m+[m[32m  bb.end();[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32m// Byte-by-byte versions[m
[32m+[m[32mfor (let test of tests) {[m
[32m+[m[32m  test = { ...test };[m
[32m+[m[32m  test.what += ' (byte-by-byte)';[m
[32m+[m[32m  active.set(test, 1);[m
[32m+[m
[32m+[m[32m  const { what } = test;[m
[32m+[m[32m  const charset = test.charset || 'utf-8';[m
[32m+[m[32m  const bb = busboy({[m
[32m+[m[32m    limits: test.limits,[m
[32m+[m[32m    headers: {[m
[32m+[m[32m      'content-type': `application/x-www-form-urlencoded; charset="${charset}"`,[m
[32m+[m[32m    },[m
[32m+[m[32m  });[m
[32m+[m[32m  const results = [];[m
[32m+[m
[32m+[m[32m  bb.on('field', (key, val, info) => {[m
[32m+[m[32m    results.push([key, val, info]);[m
[32m+[m[32m  });[m
[32m+[m
[32m+[m[32m  bb.on('file', () => {[m
[32m+[m[32m    throw new Error(`[${what}] Unexpected file`);[m
[32m+[m[32m  });[m
[32m+[m
[32m+[m[32m  bb.on('close', () => {[m
[32m+[m[32m    active.delete(test);[m
[32m+[m
[32m+[m[32m    assert.deepStrictEqual([m
[32m+[m[32m      results,[m
[32m+[m[32m      test.expected,[m
[32m+[m[32m      `[${what}] Results mismatch.\n`[m
[32m+[m[32m        + `Parsed: ${inspect(results)}\n`[m
[32m+[m[32m        + `Expected: ${inspect(test.expected)}`[m
[32m+[m[32m    );[m
[32m+[m[32m  });[m
[32m+[m
[32m+[m[32m  for (const src of test.source) {[m
[32m+[m[32m    const buf = (typeof src === 'string' ? Buffer.from(src, 'utf8') : src);[m
[32m+[m[32m    for (let i = 0; i < buf.length; ++i)[m
[32m+[m[32m      bb.write(buf.slice(i, i + 1));[m
[32m+[m[32m  }[m
[32m+[m[32m  bb.end();[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32m{[m
[32m+[m[32m  let exception = false;[m
[32m+[m[32m  process.once('uncaughtException', (ex) => {[m
[32m+[m[32m    exception = true;[m
[32m+[m[32m    throw ex;[m
[32m+[m[32m  });[m
[32m+[m[32m  process.on('exit', () => {[m
[32m+[m[32m    if (exception || active.size === 0)[m
[32m+[m[32m      return;[m
[32m+[m[32m    process.exitCode = 1;[m
[32m+[m[32m    console.error('==========================');[m
[32m+[m[32m    console.error(`${active.size} test(s) did not finish:`);[m
[32m+[m[32m    console.error('==========================');[m
[32m+[m[32m    console.error(Array.from(active.keys()).map((v) => v.what).join('\n'));[m
[32m+[m[32m  });[m
[32m+[m[32m}[m
[1mdiff --git a/node_modules/busboy/test/test.js b/node_modules/busboy/test/test.js[m
[1mnew file mode 100644[m
[1mindex 0000000..d0380f2[m
[1m--- /dev/null[m
[1m+++ b/node_modules/busboy/test/test.js[m
[36m@@ -0,0 +1,20 @@[m
[32m+[m[32m'use strict';[m
[32m+[m
[32m+[m[32mconst { spawnSync } = require('child_process');[m
[32m+[m[32mconst { readdirSync } = require('fs');[m
[32m+[m[32mconst { join } = require('path');[m
[32m+[m
[32m+[m[32mconst files = readdirSync(__dirname).sort();[m
[32m+[m[32mfor (const filename of files) {[m
[32m+[m[32m  if (filename.startsWith('test-')) {[m
[32m+[m[32m    const path = join(__dirname, filename);[m
[32m+[m[32m    console.log(`> Running ${filename} ...`);[m
[32m+[m[32m    const result = spawnSync(`${process.argv0} ${path}`, {[m
[32m+[m[32m      shell: true,[m
[32m+[m[32m      stdio: 'inherit',[m
[32m+[m[32m      windowsHide: true[m
[32m+[m[32m    });[m
[32m+[m[32m    if (result.status !== 0)[m
[32m+[m[32m      process.exitCode = 1;[m
[32m+[m[32m  }[m
[32m+[m[32m}[m
[1mdiff --git a/node_modules/concat-stream/LICENSE b/node_modules/concat-stream/LICENSE[m
[1mnew file mode 100644[m
[1mindex 0000000..99c130e[m
[1m--- /dev/null[m
[1m+++ b/node_modules/concat-stream/LICENSE[m
[36m@@ -0,0 +1,24 @@[m
[32m+[m[32mThe MIT License[m
[32m+[m
[32m+[m[32mCopyright (c) 2013 Max Ogden[m
[32m+[m
[32m+[m[32mPermission is hereby granted, free of charge,[m[41m [m
[32m+[m[32mto any person obtaining a copy of this software and[m[41m [m
[32m+[m[32massociated documentation files (the "Software"), to[m[41m [m
[32m+[m[32mdeal in the Software without restriction, including[m[41m [m
[32m+[m[32mwithout limitation the rights to use, copy, modify,[m[41m [m
[32m+[m[32mmerge, publish, distribute, sublicense, and/or sell[m[41m [m
[32m+[m[32mcopies of the Software, and to permit persons to whom[m[41m [m
[32m+[m[32mthe Software is furnished to do so,[m[41m [m
[32m+[m[32msubject to the following conditions:[m
[32m+[m
[32m+[m[32mThe above copyright notice and this permission notice[m[41m [m
[32m+[m[32mshall be included in all copies or substantial portions of the Software.[m
[32m+[m
[32m+[m[32mTHE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,[m[41m [m
[32m+[m[32mEXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES[m[41m [m
[32m+[m[32mOF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.[m[41m [m
[32m+[m[32mIN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR[m[41m [m
[32m+[m[32mANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,[m[41m [m
[32m+[m[32mTORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE[m[41m [m
[32m+[m[32mSOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.[m
\ No newline at end of file[m
[1mdiff --git a/node_modules/concat-stream/index.js b/node_modules/concat-stream/index.js[m
[1mnew file mode 100644[m
[1mindex 0000000..dd672a7[m
[1m--- /dev/null[m
[1m+++ b/node_modules/concat-stream/index.js[m
[36m@@ -0,0 +1,144 @@[m
[32m+[m[32mvar Writable = require('readable-stream').Writable[m
[32m+[m[32mvar inherits = require('inherits')[m
[32m+[m[32mvar bufferFrom = require('buffer-from')[m
[32m+[m
[32m+[m[32mif (typeof Uint8Array === 'undefined') {[m
[32m+[m[32m  var U8 = require('typedarray').Uint8Array[m
[32m+[m[32m} else {[m
[32m+[m[32m  var U8 = Uint8Array[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32mfunction ConcatStream(opts, cb) {[m
[32m+[m[32m  if (!(this instanceof ConcatStream)) return new ConcatStream(opts, cb)[m
[32m+[m
[32m+[m[32m  if (typeof opts === 'function') {[m
[32m+[m[32m    cb = opts[m
[32m+[m[32m    opts = {}[m
[32m+[m[32m  }[m
[32m+[m[32m  if (!opts) opts = {}[m
[32m+[m
[32m+[m[32m  var encoding = opts.encoding[m
[32m+[m[32m  var shouldInferEncoding = false[m
[32m+[m
[32m+[m[32m  if (!encoding) {[m
[32m+[m[32m    shouldInferEncoding = true[m
[32m+[m[32m  } else {[m
[32m+[m[32m    encoding =  String(encoding).toLowerCase()[m
[32m+[m[32m    if (encoding === 'u8' || encoding === 'uint8') {[m
[32m+[m[32m      encoding = 'uint8array'[m
[32m+[m[32m    }[m
[32m+[m[32m  }[m
[32m+[m
[32m+[m[32m  Writable.call(this, { objectMode: true })[m
[32m+[m
[32m+[m[32m  this.encoding = encoding[m
[32m+[m[32m  this.shouldInferEncoding = shouldInferEncoding[m
[32m+[m
[32m+[m[32m  if (cb) this.on('finish', function () { cb(this.getBody()) })[m
[32m+[m[32m  this.body = [][m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32mmodule.exports = ConcatStream[m
[32m+[m[32minherits(ConcatStream, Writable)[m
[32m+[m
[32m+[m[32mConcatStream.prototype._write = function(chunk, enc, next) {[m
[32m+[m[32m  this.body.push(chunk)[m
[32m+[m[32m  next()[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32mConcatStream.prototype.inferEncoding = function (buff) {[m
[32m+[m[32m  var firstBuffer = buff === undefined ? this.body[0] : buff;[m
[32m+[m[32m  if (Buffer.isBuffer(firstBuffer)) return 'buffer'[m
[32m+[m[32m  if (typeof Uint8Array !== 'undefined' && firstBuffer instanceof Uint8Array) return 'uint8array'[m
[32m+[m[32m  if (Array.isArray(firstBuffer)) return 'array'[m
[32m+[m[32m  if (typeof firstBuffer === 'string') return 'string'[m
[32m+[m[32m  if (Object.prototype.toString.call(firstBuffer) === "[object Object]") return 'object'[m
[32m+[m[32m  return 'buffer'[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32mConcatStream.prototype.getBody = function () {[m
[32m+[m[32m  if (!this.encoding && this.body.length === 0) return [][m
[32m+[m[32m  if (this.shouldInferEncoding) this.encoding = this.inferEncoding()[m
[32m+[m[32m  if (this.encoding === 'array') return arrayConcat(this.body)[m
[32m+[m[32m  if (this.encoding === 'string') return stringConcat(this.body)[m
[32m+[m[32m  if (this.encoding === 'buffer') return bufferConcat(this.body)[m
[32m+[m[32m  if (this.encoding === 'uint8array') return u8Concat(this.body)[m
[32m+[m[32m  return this.body[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32mvar isArray = Array.isArray || function (arr) {[m
[32m+[m[32m  return Object.prototype.toString.call(arr) == '[object Array]'[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32mfunction isArrayish (arr) {[m
[32m+[m[32m  return /Array\]$/.test(Object.prototype.toString.call(arr))[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32mfunction isBufferish (p) {[m
[32m+[m[32m  return typeof p === 'string' || isArrayish(p) || (p && typeof p.subarray === 'function')[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32mfunction stringConcat (parts) {[m
[32m+[m[32m  var strings = [][m
[32m+[m[32m  var needsToString = false[m
[32m+[m[32m  for (var i = 0; i < parts.length; i++) {[m
[32m+[m[32m    var p = parts[i][m
[32m+[m[32m    if (typeof p === 'string') {[m
[32m+[m[32m      strings.push(p)[m
[32m+[m[32m    } else if (Buffer.isBuffer(p)) {[m
[32m+[m[32m      strings.push(p)[m
[32m+[m[32m    } else if (isBufferish(p)) {[m
[32m+[m[32m      strings.push(bufferFrom(p))[m
[32m+[m[32m    } else {[m
[32m+[m[32m      strings.push(bufferFrom(String(p)))[m
[32m+[m[32m    }[m
[32m+[m[32m  }[m
[32m+[m[32m  if (Buffer.isBuffer(parts[0])) {[m
[32m+[m[32m    strings = Buffer.concat(strings)[m
[32m+[m[32m    strings = strings.toString('utf8')[m
[32m+[m[32m  } else {[m
[32m+[m[32m    strings = strings.join('')[m
[32m+[m[32m  }[m
[32m+[m[32m  return strings[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32mfunction bufferConcat (parts) {[m
[32m+[m[32m  var bufs = [][m
[32m+[m[32m  for (var i = 0; i < parts.length; i++) {[m
[32m+[m[32m    var p = parts[i][m
[32m+[m[32m    if (Buffer.isBuffer(p)) {[m
[32m+[m[32m      bufs.push(p)[m
[32m+[m[32m    } else if (isBufferish(p)) {[m
[32m+[m[32m      bufs.push(bufferFrom(p))[m
[32m+[m[32m    } else {[m
[32m+[m[32m      bufs.push(bufferFrom(String(p)))[m
[32m+[m[32m    }[m
[32m+[m[32m  }[m
[32m+[m[32m  return Buffer.concat(bufs)[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32mfunction arrayConcat (parts) {[m
[32m+[m[32m  var res = [][m
[32m+[m[32m  for (var i = 0; i < parts.length; i++) {[m
[32m+[m[32m    res.push.apply(res, parts[i])[m
[32m+[m[32m  }[m
[32m+[m[32m  return res[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32mfunction u8Concat (parts) {[m
[32m+[m[32m  var len = 0[m
[32m+[m[32m  for (var i = 0; i < parts.length; i++) {[m
[32m+[m[32m    if (typeof parts[i] === 'string') {[m
[32m+[m[32m      parts[i] = bufferFrom(parts[i])[m
[32m+[m[32m    }[m
[32m+[m[32m    len += parts[i].length[m
[32m+[m[32m  }[m
[32m+[m[32m  var u8 = new U8(len)[m
[32m+[m[32m  for (var i = 0, offset = 0; i < parts.length; i++) {[m
[32m+[m[32m    var part = parts[i][m
[32m+[m[32m    for (var j = 0; j < part.length; j++) {[m
[32m+[m[32m      u8[offset++] = part[j][m
[32m+[m[32m    }[m
[32m+[m[32m  }[m
[32m+[m[32m  return u8[m
[32m+[m[32m}[m
[1mdiff --git a/node_modules/concat-stream/package.json b/node_modules/concat-stream/package.json[m
[1mnew file mode 100644[m
[1mindex 0000000..f709022[m
[1m--- /dev/null[m
[1m+++ b/node_modules/concat-stream/package.json[m
[36m@@ -0,0 +1,55 @@[m
[32m+[m[32m{[m
[32m+[m[32m  "name": "concat-stream",[m
[32m+[m[32m  "version": "1.6.2",[m
[32m+[m[32m  "description": "writable stream that concatenates strings or binary data and calls a callback with the result",[m
[32m+[m[32m  "tags": [[m
[32m+[m[32m    "stream",[m
[32m+[m[32m    "simple",[m
[32m+[m[32m    "util",[m
[32m+[m[32m    "utility"[m
[32m+[m[32m  ],[m
[32m+[m[32m  "author": "Max Ogden <max@maxogden.com>",[m
[32m+[m[32m  "repository": {[m
[32m+[m[32m    "type": "git",[m
[32m+[m[32m    "url": "http://github.com/maxogden/concat-stream.git"[m
[32m+[m[32m  },[m
[32m+[m[32m  "bugs": {[m
[32m+[m[32m    "url": "http://github.com/maxogden/concat-stream/issues"[m
[32m+[m[32m  },[m
[32m+[m[32m  "engines": [[m
[32m+[m[32m    "node >= 0.8"[m
[32m+[m[32m  ],[m
[32m+[m[32m  "main": "index.js",[m
[32m+[m[32m  "files": [[m
[32m+[m[32m    "index.js"[m
[32m+[m[32m  ],[m
[32m+[m[32m  "scripts": {[m
[32m+[m[32m    "test": "tape test/*.js test/server/*.js"[m
[32m+[m[32m  },[m
[32m+[m[32m  "license": "MIT",[m
[32m+[m[32m  "dependencies": {[m
[32m+[m[32m    "buffer-from": "^1.0.0",[m
[32m+[m[32m    "inherits": "^2.0.3",[m
[32m+[m[32m    "readable-stream": "^2.2.2",[m
[32m+[m[32m    "typedarray": "^0.0.6"[m
[32m+[m[32m  },[m
[32m+[m[32m  "devDependencies": {[m
[32m+[m[32m    "tape": "^4.6.3"[m
[32m+[m[32m  },[m
[32m+[m[32m  "testling": {[m
[32m+[m[32m    "files": "test/*.js",[m
[32m+[m[32m    "browsers": [[m
[32m+[m[32m      "ie/8..latest",[m
[32m+[m[32m      "firefox/17..latest",[m
[32m+[m[32m      "firefox/nightly",[m
[32m+[m[32m      "chrome/22..latest",[m
[32m+[m[32m      "chrome/canary",[m
[32m+[m[32m      "opera/12..latest",[m
[32m+[m[32m      "opera/next",[m
[32m+[m[32m      "safari/5.1..latest",[m
[32m+[m[32m      "ipad/6.0..latest",[m
[32m+[m[32m      "iphone/6.0..latest",[m
[32m+[m[32m      "android-browser/4.2..latest"[m
[32m+[m[32m    ][m
[32m+[m[32m  }[m
[32m+[m[32m}[m
[1mdiff --git a/node_modules/concat-stream/readme.md b/node_modules/concat-stream/readme.md[m
[1mnew file mode 100644[m
[1mindex 0000000..7aa19c4[m
[1m--- /dev/null[m
[1m+++ b/node_modules/concat-stream/readme.md[m
[36m@@ -0,0 +1,102 @@[m
[32m+[m[32m# concat-stream[m
[32m+[m
[32m+[m[32mWritable stream that concatenates all the data from a stream and calls a callback with the result. Use this when you want to collect all the data from a stream into a single buffer.[m
[32m+[m
[32m+[m[32m[![Build Status](https://travis-ci.org/maxogden/concat-stream.svg?branch=master)](https://travis-ci.org/maxogden/concat-stream)[m
[32m+[m
[32m+[m[32m[![NPM](https://nodei.co/npm/concat-stream.png)](https://nodei.co/npm/concat-stream/)[m
[32m+[m
[32m+[m[32m### description[m
[32m+[m
[32m+[m[32mStreams emit many buffers. If you want to collect all of the buffers, and when the stream ends concatenate all of the buffers together and receive a single buffer then this is the module for you.[m
[32m+[m
[32m+[m[32mOnly use this if you know you can fit all of the output of your stream into a single Buffer (e.g. in RAM).[m
[32m+[m
[32m+[m[32mThere are also `objectMode` streams that emit things other than Buffers, and you can concatenate these too. See below for details.[m
[32m+[m
[32m+[m[32m## Related[m
[32m+[m
[32m+[m[32m`concat-stream` is part of the [mississippi stream utility collection](https://github.com/maxogden/mississippi) which includes more useful stream modules similar to this one.[m
[32m+[m
[32m+[m[32m### examples[m
[32m+[m
[32m+[m[32m#### Buffers[m
[32m+[m
[32m+[m[32m```js[m
[32m+[m[32mvar fs = require('fs')[m
[32m+[m[32mvar concat = require('concat-stream')[m
[32m+[m
[32m+[m[32mvar readStream = fs.createReadStream('cat.png')[m
[32m+[m[32mvar concatStream = concat(gotPicture)[m
[32m+[m
[32m+[m[32mreadStream.on('error', handleError)[m
[32m+[m[32mreadStream.pipe(concatStream)[m
[32m+[m
[32m+[m[32mfunction gotPicture(imageBuffer) {[m
[32m+[m[32m  // imageBuffer is all of `cat.png` as a node.js Buffer[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32mfunction handleError(err) {[m
[32m+[m[32m  // handle your error appropriately here, e.g.:[m
[32m+[m[32m  console.error(err) // print the error to STDERR[m
[32m+[m[32m  process.exit(1) // exit program with non-zero exit code[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32m```[m
[32m+[m
[32m+[m[32m#### Arrays[m
[32m+[m
[32m+[m[32m```js[m
[32m+[m[32mvar write = concat(function(data) {})[m
[32m+[m[32mwrite.write([1,2,3])[m
[32m+[m[32mwrite.write([4,5,6])[m
[32m+[m[32mwrite.end()[m
[32m+[m[32m// data will be [1,2,3,4,5,6] in the above callback[m
[32m+[m[32m```[m
[32m+[m
[32m+[m[32m#### Uint8Arrays[m
[32m+[m
[32m+[m[32m```js[m
[32m+[m[32mvar write = concat(function(data) {})[m
[32m+[m[32mvar a = new Uint8Array(3)[m
[32m+[m[32ma[0] = 97; a[1] = 98; a[2] = 99[m
[32m+[m[32mwrite.write(a)[m
[32m+[m[32mwrite.write('!')[m
[32m+[m[32mwrite.end(Buffer.from('!!1'))[m
[32m+[m[32m```[m
[32m+[m
[32m+[m[32mSee `test/` for more examples[m
[32m+[m
[32m+[m[32m# methods[m
[32m+[m
[32m+[m[32m```js[m
[32m+[m[32mvar concat = require('concat-stream')[m
[32m+[m[32m```[m
[32m+[m
[32m+[m[32m## var writable = concat(opts={}, cb)[m
[32m+[m
[32m+[m[32mReturn a `writable` stream that will fire `cb(data)` with all of the data that[m
[32m+[m[32mwas written to the stream. Data can be written to `writable` as strings,[m
[32m+[m[32mBuffers, arrays of byte integers, and Uint8Arrays.[m[41m [m
[32m+[m
[32m+[m[32mBy default `concat-stream` will give you back the same data type as the type of the first buffer written to the stream. Use `opts.encoding` to set what format `data` should be returned as, e.g. if you if you don't want to rely on the built-in type checking or for some other reason.[m
[32m+[m
[32m+[m[32m* `string` - get a string[m
[32m+[m[32m* `buffer` - get back a Buffer[m
[32m+[m[32m* `array` - get an array of byte integers[m
[32m+[m[32m* `uint8array`, `u8`, `uint8` - get back a Uint8Array[m
[32m+[m[32m* `object`, get back an array of Objects[m
[32m+[m
[32m+[m[32mIf you don't specify an encoding, and the types can't be inferred (e.g. you write things that aren't in the list above), it will try to convert concat them into a `Buffer`.[m
[32m+[m
[32m+[m[32mIf nothing is written to `writable` then `data` will be an empty array `[]`.[m
[32m+[m
[32m+[m[32m# error handling[m
[32m+[m
[32m+[m[32m`concat-stream` does not handle errors for you, so you must handle errors on whatever streams you pipe into `concat-stream`. This is a general rule when programming with node.js streams: always handle errors on each and every stream. Since `concat-stream` is not itself a stream it does not emit errors.[m
[32m+[m
[32m+[m[32mWe recommend using [`end-of-stream`](https://npmjs.org/end-of-stream) or [`pump`](https://npmjs.org/pump) for writing error tolerant stream code.[m
[32m+[m
[32m+[m[32m# license[m
[32m+[m
[32m+[m[32mMIT LICENSE[m
[1mdiff --git a/node_modules/core-util-is/LICENSE b/node_modules/core-util-is/LICENSE[m
[1mnew file mode 100644[m
[1mindex 0000000..d8d7f94[m
[1m--- /dev/null[m
[1m+++ b/node_modules/core-util-is/LICENSE[m
[36m@@ -0,0 +1,19 @@[m
[32m+[m[32mCopyright Node.js contributors. All rights reserved.[m
[32m+[m
[32m+[m[32mPermission is hereby granted, free of charge, to any person obtaining a copy[m
[32m+[m[32mof this software and associated documentation files (the "Software"), to[m
[32m+[m[32mdeal in the Software without restriction, including without limitation the[m
[32m+[m[32mrights to use, copy, modify, merge, publish, distribute, sublicense, and/or[m
[32m+[m[32msell copies of the Software, and to permit persons to whom the Software is[m
[32m+[m[32mfurnished to do so, subject to the following conditions:[m
[32m+[m
[32m+[m[32mThe above copyright notice and this permission notice shall be included in[m
[32m+[m[32mall copies or substantial portions of the Software.[m
[32m+[m
[32m+[m[32mTHE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR[m
[32m+[m[32mIMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,[m
[32m+[m[32mFITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE[m
[32m+[m[32mAUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER[m
[32m+[m[32mLIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING[m
[32m+[m[32mFROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS[m
[32m+[m[32mIN THE SOFTWARE.[m
[1mdiff --git a/node_modules/core-util-is/README.md b/node_modules/core-util-is/README.md[m
[1mnew file mode 100644[m
[1mindex 0000000..5a76b41[m
[1m--- /dev/null[m
[1m+++ b/node_modules/core-util-is/README.md[m
[36m@@ -0,0 +1,3 @@[m
[32m+[m[32m# core-util-is[m
[32m+[m
[32m+[m[32mThe `util.is*` functions introduced in Node v0.12.[m
[1mdiff --git a/node_modules/core-util-is/lib/util.js b/node_modules/core-util-is/lib/util.js[m
[1mnew file mode 100644[m
[1mindex 0000000..6e5a20d[m
[1m--- /dev/null[m
[1m+++ b/node_modules/core-util-is/lib/util.js[m
[36m@@ -0,0 +1,107 @@[m
[32m+[m[32m// Copyright Joyent, Inc. and other Node contributors.[m
[32m+[m[32m//[m
[32m+[m[32m// Permission is hereby granted, free of charge, to any person obtaining a[m
[32m+[m[32m// copy of this software and associated documentation files (the[m
[32m+[m[32m// "Software"), to deal in the Software without restriction, including[m
[32m+[m[32m// without limitation the rights to use, copy, modify, merge, publish,[m
[32m+[m[32m// distribute, sublicense, and/or sell copies of the Software, and to permit[m
[32m+[m[32m// persons to whom the Software is furnished to do so, subject to the[m
[32m+[m[32m// following conditions:[m
[32m+[m[32m//[m
[32m+[m[32m// The above copyright notice and this permission notice shall be included[m
[32m+[m[32m// in all copies or substantial portions of the Software.[m
[32m+[m[32m//[m
[32m+[m[32m// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS[m
[32m+[m[32m// OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF[m
[32m+[m[32m// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN[m
[32m+[m[32m// NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,[m
[32m+[m[32m// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR[m
[32m+[m[32m// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE[m
[32m+[m[32m// USE OR OTHER DEALINGS IN THE SOFTWARE.[m
[32m+[m
[32m+[m[32m// NOTE: These type checking functions intentionally don't use `instanceof`[m
[32m+[m[32m// because it is fragile and can be easily faked with `Object.create()`.[m
[32m+[m
[32m+[m[32mfunction isArray(arg) {[m
[32m+[m[32m  if (Array.isArray) {[m
[32m+[m[32m    return Array.isArray(arg);[m
[32m+[m[32m  }[m
[32m+[m[32m  return objectToString(arg) === '[object Array]';[m
[32m+[m[32m}[m
[32m+[m[32mexports.isArray = isArray;[m
[32m+[m
[32m+[m[32mfunction isBoolean(arg) {[m
[32m+[m[32m  return typeof arg === 'boolean';[m
[32m+[m[32m}[m
[32m+[m[32mexports.isBoolean = isBoolean;[m
[32m+[m
[32m+[m[32mfunction isNull(arg) {[m
[32m+[m[32m  return arg === null;[m
[32m+[m[32m}[m
[32m+[m[32mexports.isNull = isNull;[m
[32m+[m
[32m+[m[32mfunction isNullOrUndefined(arg) {[m
[32m+[m[32m  return arg == null;[m
[32m+[m[32m}[m
[32m+[m[32mexports.isNullOrUndefined = isNullOrUndefined;[m
[32m+[m
[32m+[m[32mfunction isNumber(arg) {[m
[32m+[m[32m  return typeof arg === 'number';[m
[32m+[m[32m}[m
[32m+[m[32mexports.isNumber = isNumber;[m
[32m+[m
[32m+[m[32mfunction isString(arg) {[m
[32m+[m[32m  return typeof arg === 'string';[m
[32m+[m[32m}[m
[32m+[m[32mexports.isString = isString;[m
[32m+[m
[32m+[m[32mfunction isSymbol(arg) {[m
[32m+[m[32m  return typeof arg === 'symbol';[m
[32m+[m[32m}[m
[32m+[m[32mexports.isSymbol = isSymbol;[m
[32m+[m
[32m+[m[32mfunction isUndefined(arg) {[m
[32m+[m[32m  return arg === void 0;[m
[32m+[m[32m}[m
[32m+[m[32mexports.isUndefined = isUndefined;[m
[32m+[m
[32m+[m[32mfunction isRegExp(re) {[m
[32m+[m[32m  return objectToString(re) === '[object RegExp]';[m
[32m+[m[32m}[m
[32m+[m[32mexports.isRegExp = isRegExp;[m
[32m+[m
[32m+[m[32mfunction isObject(arg) {[m
[32m+[m[32m  return typeof arg === 'object' && arg !== null;[m
[32m+[m[32m}[m
[32m+[m[32mexports.isObject = isObject;[m
[32m+[m
[32m+[m[32mfunction isDate(d) {[m
[32m+[m[32m  return objectToString(d) === '[object Date]';[m
[32m+[m[32m}[m
[32m+[m[32mexports.isDate = isDate;[m
[32m+[m
[32m+[m[32mfunction isError(e) {[m
[32m+[m[32m  return (objectToString(e) === '[object Error]' || e instanceof Error);[m
[32m+[m[32m}[m
[32m+[m[32mexports.isError = isError;[m
[32m+[m
[32m+[m[32mfunction isFunction(arg) {[m
[32m+[m[32m  return typeof arg === 'function';[m
[32m+[m[32m}[m
[32m+[m[32mexports.isFunction = isFunction;[m
[32m+[m
[32m+[m[32mfunction isPrimitive(arg) {[m
[32m+[m[32m  return arg === null ||[m
[32m+[m[32m         typeof arg === 'boolean' ||[m
[32m+[m[32m         typeof arg === 'number' ||[m
[32m+[m[32m         typeof arg === 'string' ||[m
[32m+[m[32m         typeof arg === 'symbol' ||  // ES6 symbol[m
[32m+[m[32m         typeof arg === 'undefined';[m
[32m+[m[32m}[m
[32m+[m[32mexports.isPrimitive = isPrimitive;[m
[32m+[m
[32m+[m[32mexports.isBuffer = require('buffer').Buffer.isBuffer;[m
[32m+[m
[32m+[m[32mfunction objectToString(o) {[m
[32m+[m[32m  return Object.prototype.toString.call(o);[m
[32m+[m[32m}[m
[1mdiff --git a/node_modules/core-util-is/package.json b/node_modules/core-util-is/package.json[m
[1mnew file mode 100644[m
[1mindex 0000000..b0c51f5[m
[1m--- /dev/null[m
[1m+++ b/node_modules/core-util-is/package.json[m
[36m@@ -0,0 +1,38 @@[m
[32m+[m[32m{[m
[32m+[m[32m  "name": "core-util-is",[m
[32m+[m[32m  "version": "1.0.3",[m
[32m+[m[32m  "description": "The `util.is*` functions introduced in Node v0.12.",[m
[32m+[m[32m  "main": "lib/util.js",[m
[32m+[m[32m  "files": [[m
[32m+[m[32m    "lib"[m
[32m+[m[32m  ],[m
[32m+[m[32m  "repository": {[m
[32m+[m[32m    "type": "git",[m
[32m+[m[32m    "url": "git://github.com/isaacs/core-util-is"[m
[32m+[m[32m  },[m
[32m+[m[32m  "keywords": [[m
[32m+[m[32m    "util",[m
[32m+[m[32m    "isBuffer",[m
[32m+[m[32m    "isArray",[m
[32m+[m[32m    "isNumber",[m
[32m+[m[32m    "isString",[m
[32m+[m[32m    "isRegExp",[m
[32m+[m[32m    "isThis",[m
[32m+[m[32m    "isThat",[m
[32m+[m[32m    "polyfill"[m
[32m+[m[32m  ],[m
[32m+[m[32m  "author": "Isaac Z. Schlueter <i@izs.me> (http://blog.izs.me/)",[m
[32m+[m[32m  "license": "MIT",[m
[32m+[m[32m  "bugs": {[m
[32m+[m[32m    "url": "https://github.com/isaacs/core-util-is/issues"[m
[32m+[m[32m  },[m
[32m+[m[32m  "scripts": {[m
[32m+[m[32m    "test": "tap test.js",[m
[32m+[m[32m    "preversion": "npm test",[m
[32m+[m[32m    "postversion": "npm publish",[m
[32m+[m[32m    "prepublishOnly": "git push origin --follow-tags"[m
[32m+[m[32m  },[m
[32m+[m[32m  "devDependencies": {[m
[32m+[m[32m    "tap": "^15.0.9"[m
[32m+[m[32m  }[m
[32m+[m[32m}[m
[1mdiff --git a/node_modules/isarray/.npmignore b/node_modules/isarray/.npmignore[m
[1mnew file mode 100644[m
[1mindex 0000000..3c3629e[m
[1m--- /dev/null[m
[1m+++ b/node_modules/isarray/.npmignore[m
[36m@@ -0,0 +1 @@[m
[32m+[m[32mnode_modules[m
[1mdiff --git a/node_modules/isarray/.travis.yml b/node_modules/isarray/.travis.yml[m
[1mnew file mode 100644[m
[1mindex 0000000..cc4dba2[m
[1m--- /dev/null[m
[1m+++ b/node_modules/isarray/.travis.yml[m
[36m@@ -0,0 +1,4 @@[m
[32m+[m[32mlanguage: node_js[m
[32m+[m[32mnode_js:[m
[32m+[m[32m  - "0.8"[m
[32m+[m[32m  - "0.10"[m
[1mdiff --git a/node_modules/isarray/Makefile b/node_modules/isarray/Makefile[m
[1mnew file mode 100644[m
[1mindex 0000000..787d56e[m
[1m--- /dev/null[m
[1m+++ b/node_modules/isarray/Makefile[m
[36m@@ -0,0 +1,6 @@[m
[32m+[m
[32m+[m[32mtest:[m
[32m+[m	[32m@node_modules/.bin/tape test.js[m
[32m+[m
[32m+[m[32m.PHONY: test[m
[32m+[m
[1mdiff --git a/node_modules/isarray/README.md b/node_modules/isarray/README.md[m
[1mnew file mode 100644[m
[1mindex 0000000..16d2c59[m
[1m--- /dev/null[m
[1m+++ b/node_modules/isarray/README.md[m
[36m@@ -0,0 +1,60 @@[m
[32m+[m
[32m+[m[32m# isarray[m
[32m+[m
[32m+[m[32m`Array#isArray` for older browsers.[m
[32m+[m
[32m+[m[32m[![build status](https://secure.travis-ci.org/juliangruber/isarray.svg)](http://travis-ci.org/juliangruber/isarray)[m
[32m+[m[32m[![downloads](https://img.shields.io/npm/dm/isarray.svg)](https://www.npmjs.org/package/isarray)[m
[32m+[m
[32m+[m[32m[![browser support](https://ci.testling.com/juliangruber/isarray.png)[m
[32m+[m[32m](https://ci.testling.com/juliangruber/isarray)[m
[32m+[m
[32m+[m[32m## Usage[m
[32m+[m
[32m+[m[32m```js[m
[32m+[m[32mvar isArray = require('isarray');[m
[32m+[m
[32m+[m[32mconsole.log(isArray([])); // => true[m
[32m+[m[32mconsole.log(isArray({})); // => false[m
[32m+[m[32m```[m
[32m+[m
[32m+[m[32m## Installation[m
[32m+[m
[32m+[m[32mWith [npm](http://npmjs.org) do[m
[32m+[m
[32m+[m[32m```bash[m
[32m+[m[32m$ npm install isarray[m
[32m+[m[32m```[m
[32m+[m
[32m+[m[32mThen bundle for the browser with[m
[32m+[m[32m[browserify](https://github.com/substack/browserify).[m
[32m+[m
[32m+[m[32mWith [component](http://component.io) do[m
[32m+[m
[32m+[m[32m```bash[m
[32m+[m[32m$ component install juliangruber/isarray[m
[32m+[m[32m```[m
[32m+[m
[32m+[m[32m## License[m
[32m+[m
[32m+[m[32m(MIT)[m
[32m+[m
[32m+[m[32mCopyright (c) 2013 Julian Gruber &lt;julian@juliangruber.com&gt;[m
[32m+[m
[32m+[m[32mPermission is hereby granted, free of charge, to any person obtaining a copy of[m
[32m+[m[32mthis software and associated documentation files (the "Software"), to deal in[m
[32m+[m[32mthe Software without restriction, including without limitation the rights to[m
[32m+[m[32muse, copy, modify, merge, publish, distribute, sublicense, and/or sell copies[m
[32m+[m[32mof the Software, and to permit persons to whom the Software is furnished to do[m
[32m+[m[32mso, subject to the following conditions:[m
[32m+[m
[32m+[m[32mThe above copyright notice and this permission notice shall be included in all[m
[32m+[m[32mcopies or substantial portions of the Software.[m
[32m+[m
[32m+[m[32mTHE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR[m
[32m+[m[32mIMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,[m
[32m+[m[32mFITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE[m
[32m+[m[32mAUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER[m
[32m+[m[32mLIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,[m
[32m+[m[32mOUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE[m
[32m+[m[32mSOFTWARE.[m
[1mdiff --git a/node_modules/isarray/component.json b/node_modules/isarray/component.json[m
[1mnew file mode 100644[m
[1mindex 0000000..9e31b68[m
[1m--- /dev/null[m
[1m+++ b/node_modules/isarray/component.json[m
[36m@@ -0,0 +1,19 @@[m
[32m+[m[32m{[m
[32m+[m[32m  "name" : "isarray",[m
[32m+[m[32m  "description" : "Array#isArray for older browsers",[m
[32m+[m[32m  "version" : "0.0.1",[m
[32m+[m[32m  "repository" : "juliangruber/isarray",[m
[32m+[m[32m  "homepage": "https://github.com/juliangruber/isarray",[m
[32m+[m[32m  "main" : "index.js",[m
[32m+[m[32m  "scripts" : [[m
[32m+[m[32m    "index.js"[m
[32m+[m[32m  ],[m
[32m+[m[32m  "dependencies" : {},[m
[32m+[m[32m  "keywords": ["browser","isarray","array"],[m
[32m+[m[32m  "author": {[m
[32m+[m[32m    "name": "Julian Gruber",[m
[32m+[m[32m    "email": "mail@juliangruber.com",[m
[32m+[m[32m    "url": "http://juliangruber.com"[m
[32m+[m[32m  },[m
[32m+[m[32m  "license": "MIT"[m
[32m+[m[32m}[m
[1mdiff --git a/node_modules/isarray/index.js b/node_modules/isarray/index.js[m
[1mnew file mode 100644[m
[1mindex 0000000..a57f634[m
[1m--- /dev/null[m
[1m+++ b/node_modules/isarray/index.js[m
[36m@@ -0,0 +1,5 @@[m
[32m+[m[32mvar toString = {}.toString;[m
[32m+[m
[32m+[m[32mmodule.exports = Array.isArray || function (arr) {[m
[32m+[m[32m  return toString.call(arr) == '[object Array]';[m
[32m+[m[32m};[m
[1mdiff --git a/node_modules/isarray/package.json b/node_modules/isarray/package.json[m
[1mnew file mode 100644[m
[1mindex 0000000..1a4317a[m
[1m--- /dev/null[m
[1m+++ b/node_modules/isarray/package.json[m
[36m@@ -0,0 +1,45 @@[m
[32m+[m[32m{[m
[32m+[m[32m  "name": "isarray",[m
[32m+[m[32m  "description": "Array#isArray for older browsers",[m
[32m+[m[32m  "version": "1.0.0",[m
[32m+[m[32m  "repository": {[m
[32m+[m[32m    "type": "git",[m
[32m+[m[32m    "url": "git://github.com/juliangruber/isarray.git"[m
[32m+[m[32m  },[m
[32m+[m[32m  "homepage": "https://github.com/juliangruber/isarray",[m
[32m+[m[32m  "main": "index.js",[m
[32m+[m[32m  "dependencies": {},[m
[32m+[m[32m  "devDependencies": {[m
[32m+[m[32m    "tape": "~2.13.4"[m
[32m+[m[32m  },[m
[32m+[m[32m  "keywords": [[m
[32m+[m[32m    "browser",[m
[32m+[m[32m    "isarray",[m
[32m+[m[32m    "array"[m
[32m+[m[32m  ],[m
[32m+[m[32m  "author": {[m
[32m+[m[32m    "name": "Julian Gruber",[m
[32m+[m[32m    "email": "mail@juliangruber.com",[m
[32m+[m[32m    "url": "http://juliangruber.com"[m
[32m+[m[32m  },[m
[32m+[m[32m  "license": "MIT",[m
[32m+[m[32m  "testling": {[m
[32m+[m[32m    "files": "test.js",[m
[32m+[m[32m    "browsers": [[m
[32m+[m[32m      "ie/8..latest",[m
[32m+[m[32m      "firefox/17..latest",[m
[32m+[m[32m      "firefox/nightly",[m
[32m+[m[32m      "chrome/22..latest",[m
[32m+[m[32m      "chrome/canary",[m
[32m+[m[32m      "opera/12..latest",[m
[32m+[m[32m      "opera/next",[m
[32m+[m[32m      "safari/5.1..latest",[m
[32m+[m[32m      "ipad/6.0..latest",[m
[32m+[m[32m      "iphone/6.0..latest",[m
[32m+[m[32m      "android-browser/4.2..latest"[m
[32m+[m[32m    ][m
[32m+[m[32m  },[m
[32m+[m[32m  "scripts": {[m
[32m+[m[32m    "test": "tape test.js"[m
[32m+[m[32m  }[m
[32m+[m[32m}[m
[1mdiff --git a/node_modules/isarray/test.js b/node_modules/isarray/test.js[m
[1mnew file mode 100644[m
[1mindex 0000000..e0c3444[m
[1m--- /dev/null[m
[1m+++ b/node_modules/isarray/test.js[m
[36m@@ -0,0 +1,20 @@[m
[32m+[m[32mvar isArray = require('./');[m
[32m+[m[32mvar test = require('tape');[m
[32m+[m
[32m+[m[32mtest('is array', function(t){[m
[32m+[m[32m  t.ok(isArray([]));[m
[32m+[m[32m  t.notOk(isArray({}));[m
[32m+[m[32m  t.notOk(isArray(null));[m
[32m+[m[32m  t.notOk(isArray(false));[m
[32m+[m
[32m+[m[32m  var obj = {};[m
[32m+[m[32m  obj[0] = true;[m
[32m+[m[32m  t.notOk(isArray(obj));[m
[32m+[m
[32m+[m[32m  var arr = [];[m
[32m+[m[32m  arr.foo = 'bar';[m
[32m+[m[32m  t.ok(isArray(arr));[m
[32m+[m
[32m+[m[32m  t.end();[m
[32m+[m[32m});[m
[32m+[m
[1mdiff --git a/node_modules/mkdirp/LICENSE b/node_modules/mkdirp/LICENSE[m
[1mnew file mode 100644[m
[1mindex 0000000..432d1ae[m
[1m--- /dev/null[m
[1m+++ b/node_modules/mkdirp/LICENSE[m
[36m@@ -0,0 +1,21 @@[m
[32m+[m[32mCopyright 2010 James Halliday (mail@substack.net)[m
[32m+[m
[32m+[m[32mThis project is free software released under the MIT/X11 license:[m
[32m+[m
[32m+[m[32mPermission is hereby granted, free of charge, to any person obtaining a copy[m
[32m+[m[32mof this software and associated documentation files (the "Software"), to deal[m
[32m+[m[32min the Software without restriction, including without limitation the rights[m
[32m+[m[32mto use, copy, modify, merge, publish, distribute, sublicense, and/or sell[m
[32m+[m[32mcopies of the Software, and to permit persons to whom the Software is[m
[32m+[m[32mfurnished to do so, subject to the following conditions:[m
[32m+[m
[32m+[m[32mThe above copyright notice and this permission notice shall be included in[m
[32m+[m[32mall copies or substantial portions of the Software.[m
[32m+[m
[32m+[m[32mTHE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR[m
[32m+[m[32mIMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,[m
[32m+[m[32mFITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE[m
[32m+[m[32mAUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER[m
[32m+[m[32mLIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,[m
[32m+[m[32mOUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN[m
[32m+[m[32mTHE SOFTWARE.[m
[1mdiff --git a/node_modules/mkdirp/bin/cmd.js b/node_modules/mkdirp/bin/cmd.js[m
[1mnew file mode 100644[m
[1mindex 0000000..d95de15[m
[1m--- /dev/null[m
[1m+++ b/node_modules/mkdirp/bin/cmd.js[m
[36m@@ -0,0 +1,33 @@[m
[32m+[m[32m#!/usr/bin/env node[m
[32m+[m
[32m+[m[32mvar mkdirp = require('../');[m
[32m+[m[32mvar minimist = require('minimist');[m
[32m+[m[32mvar fs = require('fs');[m
[32m+[m
[32m+[m[32mvar argv = minimist(process.argv.slice(2), {[m
[32m+[m[32m    alias: { m: 'mode', h: 'help' },[m
[32m+[m[32m    string: [ 'mode' ][m
[32m+[m[32m});[m
[32m+[m[32mif (argv.help) {[m
[32m+[m[32m    fs.createReadStream(__dirname + '/usage.txt').pipe(process.stdout);[m
[32m+[m[32m    return;[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32mvar paths = argv._.slice();[m
[32m+[m[32mvar mode = argv.mode ? parseInt(argv.mode, 8) : undefined;[m
[32m+[m
[32m+[m[32m(function next () {[m
[32m+[m[32m    if (paths.length === 0) return;[m
[32m+[m[32m    var p = paths.shift();[m
[32m+[m[41m    [m
[32m+[m[32m    if (mode === undefined) mkdirp(p, cb)[m
[32m+[m[32m    else mkdirp(p, mode, cb)[m
[32m+[m[41m    [m
[32m+[m[32m    function cb (err) {[m
[32m+[m[32m        if (err) {[m
[32m+[m[32m            console.error(err.message);[m
[32m+[m[32m            process.exit(1);[m
[32m+[m[32m        }[m
[32m+[m[32m        else next();[m
[32m+[m[32m    }[m
[32m+[m[32m})();[m
[1mdiff --git a/node_modules/mkdirp/bin/usage.txt b/node_modules/mkdirp/bin/usage.txt[m
[1mnew file mode 100644[m
[1mindex 0000000..f952aa2[m
[1m--- /dev/null[m
[1m+++ b/node_modules/mkdirp/bin/usage.txt[m
[36m@@ -0,0 +1,12 @@[m
[32m+[m[32musage: mkdirp [DIR1,DIR2..] {OPTIONS}[m
[32m+[m
[32m+[m[32m  Create each supplied directory including any necessary parent directories that[m
[32m+[m[32m  don't yet exist.[m
[32m+[m[41m  [m
[32m+[m[32m  If the directory already exists, do nothing.[m
[32m+[m
[32m+[m[32mOPTIONS are:[m
[32m+[m
[32m+[m[32m  -m, --mode   If a directory needs to be created, set the mode as an octal[m
[32m+[m[32m               permission string.[m
[32m+[m
[1mdiff --git a/node_modules/mkdirp/index.js b/node_modules/mkdirp/index.js[m
[1mnew file mode 100644[m
[1mindex 0000000..0890ac3[m
[1m--- /dev/null[m
[1m+++ b/node_modules/mkdirp/index.js[m
[36m@@ -0,0 +1,102 @@[m
[32m+[m[32mvar path = require('path');[m
[32m+[m[32mvar fs = require('fs');[m
[32m+[m[32mvar _0777 = parseInt('0777', 8);[m
[32m+[m
[32m+[m[32mmodule.exports = mkdirP.mkdirp = mkdirP.mkdirP = mkdirP;[m
[32m+[m
[32m+[m[32mfunction mkdirP (p, opts, f, made) {[m
[32m+[m[32m    if (typeof opts === 'function') {[m
[32m+[m[32m        f = opts;[m
[32m+[m[32m        opts = {};[m
[32m+[m[32m    }[m
[32m+[m[32m    else if (!opts || typeof opts !== 'object') {[m
[32m+[m[32m        opts = { mode: opts };[m
[32m+[m[32m    }[m
[32m+[m[41m    [m
[32m+[m[32m    var mode = opts.mode;[m
[32m+[m[32m    var xfs = opts.fs || fs;[m
[32m+[m[41m    [m
[32m+[m[32m    if (mode === undefined) {[m
[32m+[m[32m        mode = _0777[m
[32m+[m[32m    }[m
[32m+[m[32m    if (!made) made = null;[m
[32m+[m[41m    [m
[32m+[m[32m    var cb = f || /* istanbul ignore next */ function () {};[m
[32m+[m[32m    p = path.resolve(p);[m
[32m+[m[41m    [m
[32m+[m[32m    xfs.mkdir(p, mode, function (er) {[m
[32m+[m[32m        if (!er) {[m
[32m+[m[32m            made = made || p;[m
[32m+[m[32m            return cb(null, made);[m
[32m+[m[32m        }[m
[32m+[m[32m        switch (er.code) {[m
[32m+[m[32m            case 'ENOENT':[m
[32m+[m[32m                /* istanbul ignore if */[m
[32m+[m[32m                if (path.dirname(p) === p) return cb(er);[m
[32m+[m[32m                mkdirP(path.dirname(p), opts, function (er, made) {[m
[32m+[m[32m                    /* istanbul ignore if */[m
[32m+[m[32m                    if (er) cb(er, made);[m
[32m+[m[32m                    else mkdirP(p, opts, cb, made);[m
[32m+[m[32m                });[m
[32m+[m[32m                break;[m
[32m+[m
[32m+[m[32m            // In the case of any other error, just see if there's a dir[m
[32m+[m[32m            // there already.  If so, then hooray!  If not, then something[m
[32m+[m[32m            // is borked.[m
[32m+[m[32m            default:[m
[32m+[m[32m                xfs.stat(p, function (er2, stat) {[m
[32m+[m[32m                    // if the stat fails, then that's super weird.[m
[32m+[m[32m                    // let the original error be the failure reason.[m
[32m+[m[32m                    if (er2 || !stat.isDirectory()) cb(er, made)[m
[32m+[m[32m                    else cb(null, made);[m
[32m+[m[32m                });[m
[32m+[m[32m                break;[m
[32m+[m[32m        }[m
[32m+[m[32m    });[m
[32m+[m[32m}[m
[32m+[m
[32m+[m[32mmkdirP.sync = function sync (p, opts, made) {[m
[32m+[m[32m    if (!opts || typeof opts !== 'object') {[m
[32m+[m[32m        opts = { mode: opts };[m
[32m+[m[32m    }[m
[32m+[m[41m    [m
[32m+[m[32m    var mode = opts.mode;[m
[32m+[m[32m    var xfs = opts.fs || fs;[m
[32m+[m[41m    [m
[32m+[m[32m    if (mode === undefined) {[m
[32m+[m[32m        mode = _0777[m
[32m+[m[32m    }[m
[32m+[m[32m    if (!made) made = null;[m
[32m+[m
[32m+[m[32m    p = path.resolve(p);[m
[32m+[m
[32m+[m[32m    try {[m
[32m+[m[32m        xfs.mkdirSync(p, mode);[m
[32m+[m[32m        made = made || p;[m
[32m+[m[32m    }[m
[32m+[m[32m    catch (err0) {[m
[32m+[m[32m        switch (err0.code) {[m
[32m+[m[32m            case 'ENOENT' :[m
[32m+[m[32m                made = sync(path.dirname(p), opts, made);[m
[32m+[m[32m                sync(p, opts, made);[m
[32m+[m[32m                break;[m
[32m+[m
[32m+[m[32m            // In the case of any other error, just see if there's a dir[m
[32m+[m[32m            // there already.  If so, then hooray!  If not, then something[m
[32m+[m[32m            // is borked.[m
[32m+[m[32m            default:[m
[32m+[m[32m                var stat;[m
[32m+[m[32m                try {[m
[32m+[m[32m                    stat = xfs.statSync(p);[m
[32m+[m[32m                }[m
[32m+[m[32m                catch (err1) /* istanbul ignore next */ {[m
[32m+[m[32m                    throw err0;[m
[32m+[m[32m                }[m
[32m+[m[32m                /* istanbul ignore if */[m
[32m+[m[32m                if (!stat.isDirectory()) throw err0;[m
[32m+[m[32m                break;[m
[32m+[m[32m        }[m
[32m+[m[32m    }[m
[32m+[m
[32m+[m[32m    return made;[m
[32m+[m[32m};[m
[1mdiff --git a/node_modules/mkdirp/package.json b/node_modules/mkdirp/package.json[m
[1mnew file mode 100644[m
[1mindex 0000000..951e58d[m
[1m--- /dev/null[m
[1m+++ b/node_modules/mkdirp/package.json[m
[36m@@ -0,0 +1,33 @@[m
[32m+[m[32m{[m
[32m+[m[32m  "name": "mkdirp",[m
[32m+[m[32m  "description": "Recursively mkdir, like `mkdir -p`",[m
[32m+[m[32m  "version": "0.5.6",[m
[32m+[m[32m  "publishConfig": {[m
[32m+[m[32m    "tag": "legacy"[m
[32m+[m[32m  },[m
[32m+[m[32m  "author": "James Halliday <mail@substack.net> (http://substack.net)",[m
[32m+[m[32m  "main": "index.js",[m
[32m+[m[32m  "keywords": [[m
[32m+[m[32m    "mkdir",[m
[32m+[m[32m    "directory"[m
[32m+[m[32m  ],[m
[32m+[m[32m  "repository": {[m
[32m+[m[32m    "type": "git",[m
[32m+[m[32m    "url": "https://github.com/substack/node-mkdirp.git"[m
[32m+[m[32m  },[m
[32m+[m[32m  "scripts": {[m
[32m+[m[32m    "test": "tap test/*.js"[m
[32m+[m[32m  },[m
[32m+[m[32m  "dependencies": {[m
[32m+[m[32m    "minimist": "^1.2.6"[m
[32m+[m[32m  },[m
[32m+[m[32m  "devDependencies": {[m
[32m+[m[32m    "tap": "^16.0.1"[m
[32m+[m[32m  },[m
[32m+[m[32m  "bin": "bin/cmd.js",[m
[32m+[m[32m  "license": "MIT",[m
[32m+[m[32m  "files": [[m
[32m+[m[32m    "bin",[m
[32m+[m[32m    "index.js"[m
[32m+[m[32m  ][m
[32m+[m[32m}[m
[1mdiff --git a/node_modules/mkdirp/readme.markdown b/node_modules/mkdirp/readme.markdown[m
[1mnew file mode 100644[m
[1mindex 0000000..fc314bf[m
[1m--- /dev/null[m
[1m+++ b/node_modules/mkdirp/readme.markdown[m
[36m@@ -0,0 +1,100 @@[m
[32m+[m[32m# mkdirp[m
[32m+[m
[32m+[m[32mLike `mkdir -p`, but in node.js![m
[32m+[m
[32m+[m[32m[![build status](https://secure.travis-ci.org/substack/node-mkdirp.png)](http://travis-ci.org/substack/node-mkdirp)[m
[32m+[m
[32m+[m[32m# example[m
[32m+[m
[32m+[m[32m## pow.js[m
[32m+[m
[32m+[m[32m```js[m
[32m+[m[32mvar mkdirp = require('mkdirp');[m
[32m+[m[41m    [m
[32m+[m[32mmkdirp('/tmp/foo/bar/baz', function (err) {[m
[32m+[m[32m    if (err) console.error(err)[m
[32m+[m[32m    else console.log('pow!')[m
[32m+[m[32m});[m
[32m+[m[32m```[m
[32m+[m
[32m+[m[32mOutput[m
[32m+[m
[32m+[m[32m```[m
[32m+[m[32mpow![m
[32m+[m[32m```[m
[32m+[m
[32m+[m[32mAnd now /tmp/foo/bar/baz exists, huzzah![m
[32m+[m
[32m+[m[32m# methods[m
[32m+[m
[32m+[m[32m```js[m
[32m+[m[32mvar mkdirp = require('mkdirp');[m
[32m+[m[32m```[m
[32m+[m
[32m+[m[32m## mkdirp(dir, opts, cb)[m
[32m+[m
[32m+[m[32mCreate a new directory and any necessary subdirectories at `dir` with octal[m
[32m+[m[32mpermission string `opts.mode`. If `opts` is a non-object, it will be treated as[m
[32m+[m[32mthe `opts.mode`.[m
[32m+[m
[32m+[m[32mIf `opts.mode` isn't specified, it defaults to `0777`.[m
[32m+[m
[32m+[m[32m`cb(err, made)` fires with the error or the first directory `made`[m
[32m+[m[32mthat had to be created, if any.[m
[32m+[m
[32m+[m[32mYou can optionally pass in an alternate `fs` implementation by passing in[m
[32m+[m[32m`opts.fs`. Your implementation should have `opts.fs.mkdir(path, mode, cb)` and[m
[32m+[m[32m`opts.fs.stat(path, cb)`.[m
[32m+[m
[32m+[m[32m## mkdirp.sync(dir, opts)[m
[32m+[m
[32m+[m[32mSynchronously create a new directory and any necessary subdirectories at `dir`[m
[32m+[m[32mwith octal permission string `opts.mode`. If `opts` is a non-object, it will be[m
[32m+[m[32mtreated as the `opts.mode`.[m
[32m+[m
[32m+[m[32mIf `opts.mode` isn't specified, it defaults to `0777`.[m
[32m+[m
[32m+[m[32mReturns the first directory that had to be created, if any.[m
[32m+[m
[32m+[m[32mYou can optionally pass in an alternate `fs` implementation by passing in[m
[32m+[m[32m`opts.fs`. Your implementation should have `opts.fs.mkdirSync(path, mode)` and[m
[32m+[m[32m`opts.fs.statSync(path)`.[m
[32m+[m
[32m+[m[32m# usage[m
[32m+[m
[32m+[m[32mThis package also ships with a `mkdirp` command.[m
[32m+[m
[32m+[m[32m```[m
[32m+[m[32musage: mkdirp [DIR1,DIR2..] {OPTIONS}[m
[32m+[m
[32m+[m[32m  Create each supplied directory including any necessary parent directories that[m
[32m+[m[32m  don't yet exist.[m
[32m+[m[41m  [m
[32m+[m[32m  If the directory already exists, do nothing.[m
[32m+[m
[32m+[m[32mOPTIONS are:[m
[32m+[m
[32m+[m[32m  -m, --mode   If a directory needs to be created, set the mode as an octal[m
[32m+[m[32m               permission string.[m
[32m+[m
[32m+[m[32m```[m
[32m+[m
[32m+[m[32m# install[m
[32m+[m
[32m+[m[32mWith [npm](http://npmjs.org) do:[m
[32m+[m
[32m+[m[32m```[m
[32m+[m[32mnpm install mkdirp[m
[32m+[m[32m```[m
[32m+[m
[32m+[m[32mto get the library, or[m
[32m+[m
[32m+[m[32m```[m
[32m+[m[32mnpm install -g mkdirp[m
[32m+[m[32m```[m
[32m+[m
[32m+[m[32mto get the command.[m
[32m+[m
[32m+[m[32m# license[m
[32m+[m
[32m+[m[32mMIT[m
[1mdiff --git a/node_modules/multer/LICENSE b/node_modules/multer/LICENSE[m
[1mnew file mode 100644[m
[1mindex 0000000..6c011b1[m
[1m--- /dev/null[m
[1m+++ b/node_modules/multer/LICENSE[m
[36m@@ -0,0 +1,17 @@[m
[32m+[m[32mCopyright (c) 2014 Hage Yaapa <[http://www.hacksparrow.com](http://www.hacksparrow.com)>[m
[32m+[m
[32m+[m[32mPermission is hereby granted, free of charge, to any person obtaining a copy[m
[32m+[m[32mof this software and associated do