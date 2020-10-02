// A '.tsx' file enables JSX support in the TypeScript compiler, 
// for more information see the following page on the TypeScript wiki:
// https://github.com/Microsoft/TypeScript/wiki/JSX

import * as React from "react";
import * as ReactDOM from "react-dom";
import ReactNotification from 'react-notifications-component';
import { MenuBox } from "./MenuBox";

ReactDOM.render(
    <div>
        <ReactNotification />
        <MenuBox />
    </div>,
   
    document.getElementById("foodorder")
);