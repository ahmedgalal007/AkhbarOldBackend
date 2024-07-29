Ext.data.JsonP.CKEDITOR_plugins_lineutils_finder({"tagname":"class","name":"CKEDITOR.plugins.lineutils.finder","autodetected":{},"files":[{"filename":"plugin.js","href":"plugin11.html#CKEDITOR-plugins-lineutils-finder"}],"private":true,"members":[{"name":"lookups","tagname":"property","owner":"CKEDITOR.plugins.lineutils.finder","id":"property-lookups","meta":{}},{"name":"relations","tagname":"property","owner":"CKEDITOR.plugins.lineutils.finder","id":"property-relations","meta":{"readonly":true}},{"name":"constructor","tagname":"method","owner":"CKEDITOR.plugins.lineutils.finder","id":"method-constructor","meta":{}},{"name":"getRange","tagname":"method","owner":"CKEDITOR.plugins.lineutils.finder","id":"method-getRange","meta":{}},{"name":"greedySearch","tagname":"method","owner":"CKEDITOR.plugins.lineutils.finder","id":"method-greedySearch","meta":{}},{"name":"pixelSearch","tagname":"method","owner":"CKEDITOR.plugins.lineutils.finder","id":"method-pixelSearch","meta":{}},{"name":"start","tagname":"method","owner":"CKEDITOR.plugins.lineutils.finder","id":"method-start","meta":{}},{"name":"stop","tagname":"method","owner":"CKEDITOR.plugins.lineutils.finder","id":"method-stop","meta":{}},{"name":"store","tagname":"method","owner":"CKEDITOR.plugins.lineutils.finder","id":"method-store","meta":{}},{"name":"traverseSearch","tagname":"method","owner":"CKEDITOR.plugins.lineutils.finder","id":"method-traverseSearch","meta":{}}],"alternateClassNames":[],"aliases":{},"id":"class-CKEDITOR.plugins.lineutils.finder","component":false,"superclasses":[],"subclasses":[],"mixedInto":[],"mixins":[],"parentMixins":[],"requires":[],"uses":[],"html":"<div><pre class=\"hierarchy\"><h4>Files</h4><div class='dependency'><a href='source/plugin11.html#CKEDITOR-plugins-lineutils-finder' target='_blank'>plugin.js</a></div></pre><div class='doc-contents'><div class='rounded-box private-box'><p><strong>NOTE:</strong> This is a private utility class for internal use by the framework. Don't rely on its existence.</p></div><p>A utility that traverses the DOM tree and discovers elements\n(relations) matching user-defined lookups.</p>\n</div><div class='members'><div class='members-section'><div class='definedBy'>Defined By</div><h3 class='members-title icon-property'>Properties</h3><div class='subsection'><div id='property-lookups' class='member first-child not-inherited'><a href='#' class='side expandable'><span>&nbsp;</span></a><div class='title'><div class='meta'><span class='defined-in' rel='CKEDITOR.plugins.lineutils.finder'>CKEDITOR.plugins.lineutils.finder</span><br/><a href='source/plugin11.html#CKEDITOR-plugins-lineutils-finder-property-lookups' target='_blank' class='view-source'>view source</a></div><a href='#!/api/CKEDITOR.plugins.lineutils.finder-property-lookups' class='name expandable'>lookups</a> : Object<span class=\"signature\"></span></div><div class='description'><div class='short'>A set of user-defined functions used by Finder to check if an element\nis a valid relation, belonging to relations. ...</div><div class='long'><p>A set of user-defined functions used by Finder to check if an element\nis a valid relation, belonging to <a href=\"#!/api/CKEDITOR.plugins.lineutils.finder-property-relations\" rel=\"CKEDITOR.plugins.lineutils.finder-property-relations\" class=\"docClass\">relations</a>.\nWhen the criterion is met, lookup returns a logical conjunction of <code><a href=\"#!/api/CKEDITOR-property-LINEUTILS_BEFORE\" rel=\"CKEDITOR-property-LINEUTILS_BEFORE\" class=\"docClass\">CKEDITOR.LINEUTILS_BEFORE</a></code>,\n<code><a href=\"#!/api/CKEDITOR-property-LINEUTILS_AFTER\" rel=\"CKEDITOR-property-LINEUTILS_AFTER\" class=\"docClass\">CKEDITOR.LINEUTILS_AFTER</a></code> or <code><a href=\"#!/api/CKEDITOR-property-LINEUTILS_INSIDE\" rel=\"CKEDITOR-property-LINEUTILS_INSIDE\" class=\"docClass\">CKEDITOR.LINEUTILS_INSIDE</a></code>.</p>\n\n<p>Lookups are passed along with Finder's definition.</p>\n\n<pre><code>lookups: {\n    'some lookup': function( el ) {\n        if ( someCondition )\n            return <a href=\"#!/api/CKEDITOR-property-LINEUTILS_BEFORE\" rel=\"CKEDITOR-property-LINEUTILS_BEFORE\" class=\"docClass\">CKEDITOR.LINEUTILS_BEFORE</a>;\n    },\n    ...\n}\n</code></pre>\n</div></div></div><div id='property-relations' class='member  not-inherited'><a href='#' class='side expandable'><span>&nbsp;</span></a><div class='title'><div class='meta'><span class='defined-in' rel='CKEDITOR.plugins.lineutils.finder'>CKEDITOR.plugins.lineutils.finder</span><br/><a href='source/plugin11.html#CKEDITOR-plugins-lineutils-finder-property-relations' target='_blank' class='view-source'>view source</a></div><a href='#!/api/CKEDITOR.plugins.lineutils.finder-property-relations' class='name expandable'>relations</a> : Object<span class=\"signature\"><span class='readonly' >readonly</span></span></div><div class='description'><div class='short'>Relations express elements in DOM that match user-defined lookups. ...</div><div class='long'><p>Relations express elements in DOM that match user-defined <a href=\"#!/api/CKEDITOR.plugins.lineutils.finder-property-lookups\" rel=\"CKEDITOR.plugins.lineutils.finder-property-lookups\" class=\"docClass\">lookups</a>.\nEvery relation has its own <code>type</code> that determines whether\nit refers to the space before, after or inside the <code>element</code>.\nThis object stores relations found by <a href=\"#!/api/CKEDITOR.plugins.lineutils.finder-method-traverseSearch\" rel=\"CKEDITOR.plugins.lineutils.finder-method-traverseSearch\" class=\"docClass\">traverseSearch</a> or <a href=\"#!/api/CKEDITOR.plugins.lineutils.finder-method-greedySearch\" rel=\"CKEDITOR.plugins.lineutils.finder-method-greedySearch\" class=\"docClass\">greedySearch</a>, structured\nin the following way:</p>\n\n<pre><code>relations: {\n    // Unique identifier of the element.\n    Number: {\n        // Element of this relation.\n        element: <a href=\"#!/api/CKEDITOR.dom.element\" rel=\"CKEDITOR.dom.element\" class=\"docClass\">CKEDITOR.dom.element</a>\n        // Conjunction of <a href=\"#!/api/CKEDITOR-property-LINEUTILS_BEFORE\" rel=\"CKEDITOR-property-LINEUTILS_BEFORE\" class=\"docClass\">CKEDITOR.LINEUTILS_BEFORE</a>, <a href=\"#!/api/CKEDITOR-property-LINEUTILS_AFTER\" rel=\"CKEDITOR-property-LINEUTILS_AFTER\" class=\"docClass\">CKEDITOR.LINEUTILS_AFTER</a> and <a href=\"#!/api/CKEDITOR-property-LINEUTILS_INSIDE\" rel=\"CKEDITOR-property-LINEUTILS_INSIDE\" class=\"docClass\">CKEDITOR.LINEUTILS_INSIDE</a>.\n        type: Number\n    },\n    ...\n}\n</code></pre>\n</div></div></div></div></div><div class='members-section'><div class='definedBy'>Defined By</div><h3 class='members-title icon-method'>Methods</h3><div class='subsection'><div id='method-constructor' class='member first-child not-inherited'><a href='#' class='side expandable'><span>&nbsp;</span></a><div class='title'><div class='meta'><span class='defined-in' rel='CKEDITOR.plugins.lineutils.finder'>CKEDITOR.plugins.lineutils.finder</span><br/><a href='source/plugin11.html#CKEDITOR-plugins-lineutils-finder-method-constructor' target='_blank' class='view-source'>view source</a></div><strong class='new-keyword'>new</strong><a href='#!/api/CKEDITOR.plugins.lineutils.finder-method-constructor' class='name expandable'>CKEDITOR.plugins.lineutils.finder</a>( <span class='pre'>editor, def</span> ) : <a href=\"#!/api/CKEDITOR.plugins.lineutils.finder\" rel=\"CKEDITOR.plugins.lineutils.finder\" class=\"docClass\">CKEDITOR.plugins.lineutils.finder</a><span class=\"signature\"></span></div><div class='description'><div class='short'>Creates a Finder class instance. ...</div><div class='long'><p>Creates a Finder class instance.</p>\n        <p>Available since: <b>4.3</b></p>\n<h3 class=\"pa\">Parameters</h3><ul><li><span class='pre'>editor</span> : <a href=\"#!/api/CKEDITOR.editor\" rel=\"CKEDITOR.editor\" class=\"docClass\">CKEDITOR.editor</a><div class='sub-desc'><p>Editor instance that the Finder belongs to.</p>\n</div></li><li><span class='pre'>def</span> : Object<div class='sub-desc'><p>Finder's definition.</p>\n</div></li></ul><h3 class='pa'>Returns</h3><ul><li><span class='pre'><a href=\"#!/api/CKEDITOR.plugins.lineutils.finder\" rel=\"CKEDITOR.plugins.lineutils.finder\" class=\"docClass\">CKEDITOR.plugins.lineutils.finder</a></span><div class='sub-desc'>\n</div></li></ul></div></div></div><div id='method-getRange' class='member  not-inherited'><a href='#' class='side expandable'><span>&nbsp;</span></a><div class='title'><div class='meta'><span class='defined-in' rel='CKEDITOR.plugins.lineutils.finder'>CKEDITOR.plugins.lineutils.finder</span><br/><a href='source/plugin11.html#CKEDITOR-plugins-lineutils-finder-method-getRange' target='_blank' class='view-source'>view source</a></div><a href='#!/api/CKEDITOR.plugins.lineutils.finder-method-getRange' class='name expandable'>getRange</a>( <span class='pre'>location</span> ) : <a href=\"#!/api/CKEDITOR.dom.range\" rel=\"CKEDITOR.dom.range\" class=\"docClass\">CKEDITOR.dom.range</a><span class=\"signature\"></span></div><div class='description'><div class='short'>Returns a range representing the relation, according to its element\nand type. ...</div><div class='long'><p>Returns a range representing the relation, according to its element\nand type.</p>\n<h3 class=\"pa\">Parameters</h3><ul><li><span class='pre'>location</span> : Object<div class='sub-desc'><p>Location containing a unique identifier and type.</p>\n</div></li></ul><h3 class='pa'>Returns</h3><ul><li><span class='pre'><a href=\"#!/api/CKEDITOR.dom.range\" rel=\"CKEDITOR.dom.range\" class=\"docClass\">CKEDITOR.dom.range</a></span><div class='sub-desc'><p>Range representing the relation.</p>\n</div></li></ul></div></div></div><div id='method-greedySearch' class='member  not-inherited'><a href='#' class='side expandable'><span>&nbsp;</span></a><div class='title'><div class='meta'><span class='defined-in' rel='CKEDITOR.plugins.lineutils.finder'>CKEDITOR.plugins.lineutils.finder</span><br/><a href='source/plugin11.html#CKEDITOR-plugins-lineutils-finder-method-greedySearch' target='_blank' class='view-source'>view source</a></div><a href='#!/api/CKEDITOR.plugins.lineutils.finder-method-greedySearch' class='name expandable'>greedySearch</a>( <span class='pre'></span> ) : Object<span class=\"signature\"></span></div><div class='description'><div class='short'>Unlike traverseSearch, it collects all elements from editable's DOM tree\nand runs lookups for every one of them, coll...</div><div class='long'><p>Unlike <a href=\"#!/api/CKEDITOR.plugins.lineutils.finder-method-traverseSearch\" rel=\"CKEDITOR.plugins.lineutils.finder-method-traverseSearch\" class=\"docClass\">traverseSearch</a>, it collects <strong>all</strong> elements from editable's DOM tree\nand runs lookups for every one of them, collecting relations.</p>\n<h3 class='pa'>Returns</h3><ul><li><span class='pre'>Object</span><div class='sub-desc'><p><a href=\"#!/api/CKEDITOR.plugins.lineutils.finder-property-relations\" rel=\"CKEDITOR.plugins.lineutils.finder-property-relations\" class=\"docClass\">relations</a>.</p>\n</div></li></ul></div></div></div><div id='method-pixelSearch' class='member  not-inherited'><a href='#' class='side expandable'><span>&nbsp;</span></a><div class='title'><div class='meta'><span class='defined-in' rel='CKEDITOR.plugins.lineutils.finder'>CKEDITOR.plugins.lineutils.finder</span><br/><a href='source/plugin11.html#CKEDITOR-plugins-lineutils-finder-method-pixelSearch' target='_blank' class='view-source'>view source</a></div><a href='#!/api/CKEDITOR.plugins.lineutils.finder-method-pixelSearch' class='name expandable'>pixelSearch</a>( <span class='pre'>el, [x], [y]</span> )<span class=\"signature\"></span></div><div class='description'><div class='short'>Iterates vertically pixel-by-pixel within a given element starting\nfrom given coordinates, searching for elements in ...</div><div class='long'><p>Iterates vertically pixel-by-pixel within a given element starting\nfrom given coordinates, searching for elements in the neighborhood.\nOnce an element is found it is processed by <a href=\"#!/api/CKEDITOR.plugins.lineutils.finder-method-traverseSearch\" rel=\"CKEDITOR.plugins.lineutils.finder-method-traverseSearch\" class=\"docClass\">traverseSearch</a>.</p>\n<h3 class=\"pa\">Parameters</h3><ul><li><span class='pre'>el</span> : <a href=\"#!/api/CKEDITOR.dom.element\" rel=\"CKEDITOR.dom.element\" class=\"docClass\">CKEDITOR.dom.element</a><div class='sub-desc'><p>Element which is the starting point.</p>\n</div></li><li><span class='pre'>x</span> : Number (optional)<div class='sub-desc'><p>Horizontal mouse coordinate relative to the viewport.</p>\n</div></li><li><span class='pre'>y</span> : Number (optional)<div class='sub-desc'><p>Vertical mouse coordinate relative to the viewport.</p>\n</div></li></ul></div></div></div><div id='method-start' class='member  not-inherited'><a href='#' class='side expandable'><span>&nbsp;</span></a><div class='title'><div class='meta'><span class='defined-in' rel='CKEDITOR.plugins.lineutils.finder'>CKEDITOR.plugins.lineutils.finder</span><br/><a href='source/plugin11.html#CKEDITOR-plugins-lineutils-finder-method-start' target='_blank' class='view-source'>view source</a></div><a href='#!/api/CKEDITOR.plugins.lineutils.finder-method-start' class='name expandable'>start</a>( <span class='pre'>[callback]</span> )<span class=\"signature\"></span></div><div class='description'><div class='short'>Initializes searching for elements with every mousemove event fired. ...</div><div class='long'><p>Initializes searching for elements with every mousemove event fired.\nTo stop searching use <a href=\"#!/api/CKEDITOR.plugins.lineutils.finder-method-stop\" rel=\"CKEDITOR.plugins.lineutils.finder-method-stop\" class=\"docClass\">stop</a>.</p>\n<h3 class=\"pa\">Parameters</h3><ul><li><span class='pre'>callback</span> : Function (optional)<div class='sub-desc'><p>Function executed on every iteration.</p>\n</div></li></ul></div></div></div><div id='method-stop' class='member  not-inherited'><a href='#' class='side expandable'><span>&nbsp;</span></a><div class='title'><div class='meta'><span class='defined-in' rel='CKEDITOR.plugins.lineutils.finder'>CKEDITOR.plugins.lineutils.finder</span><br/><a href='source/plugin11.html#CKEDITOR-plugins-lineutils-finder-method-stop' target='_blank' class='view-source'>view source</a></div><a href='#!/api/CKEDITOR.plugins.lineutils.finder-method-stop' class='name expandable'>stop</a>( <span class='pre'></span> )<span class=\"signature\"></span></div><div class='description'><div class='short'>Stops observing mouse events attached by start. ...</div><div class='long'><p>Stops observing mouse events attached by <a href=\"#!/api/CKEDITOR.plugins.lineutils.finder-method-start\" rel=\"CKEDITOR.plugins.lineutils.finder-method-start\" class=\"docClass\">start</a>.</p>\n</div></div></div><div id='method-store' class='member  not-inherited'><a href='#' class='side expandable'><span>&nbsp;</span></a><div class='title'><div class='meta'><span class='defined-in' rel='CKEDITOR.plugins.lineutils.finder'>CKEDITOR.plugins.lineutils.finder</span><br/><a href='source/plugin11.html#CKEDITOR-plugins-lineutils-finder-method-store' target='_blank' class='view-source'>view source</a></div><a href='#!/api/CKEDITOR.plugins.lineutils.finder-method-store' class='name expandable'>store</a>( <span class='pre'>el, type</span> )<span class=\"signature\"></span></div><div class='description'><div class='short'>Stores given relation in a relations object. ...</div><div class='long'><p>Stores given relation in a <a href=\"#!/api/CKEDITOR.plugins.lineutils.finder-property-relations\" rel=\"CKEDITOR.plugins.lineutils.finder-property-relations\" class=\"docClass\">relations</a> object. Processes the relation\nto normalize and avoid duplicates.</p>\n<h3 class=\"pa\">Parameters</h3><ul><li><span class='pre'>el</span> : <a href=\"#!/api/CKEDITOR.dom.element\" rel=\"CKEDITOR.dom.element\" class=\"docClass\">CKEDITOR.dom.element</a><div class='sub-desc'><p>Element of the relation.</p>\n</div></li><li><span class='pre'>type</span> : Number<div class='sub-desc'><p>Relation, one of <code><a href=\"#!/api/CKEDITOR-property-LINEUTILS_AFTER\" rel=\"CKEDITOR-property-LINEUTILS_AFTER\" class=\"docClass\">CKEDITOR.LINEUTILS_AFTER</a></code>, <code><a href=\"#!/api/CKEDITOR-property-LINEUTILS_BEFORE\" rel=\"CKEDITOR-property-LINEUTILS_BEFORE\" class=\"docClass\">CKEDITOR.LINEUTILS_BEFORE</a></code>, <code><a href=\"#!/api/CKEDITOR-property-LINEUTILS_INSIDE\" rel=\"CKEDITOR-property-LINEUTILS_INSIDE\" class=\"docClass\">CKEDITOR.LINEUTILS_INSIDE</a></code>.</p>\n</div></li></ul></div></div></div><div id='method-traverseSearch' class='member  not-inherited'><a href='#' class='side expandable'><span>&nbsp;</span></a><div class='title'><div class='meta'><span class='defined-in' rel='CKEDITOR.plugins.lineutils.finder'>CKEDITOR.plugins.lineutils.finder</span><br/><a href='source/plugin11.html#CKEDITOR-plugins-lineutils-finder-method-traverseSearch' target='_blank' class='view-source'>view source</a></div><a href='#!/api/CKEDITOR.plugins.lineutils.finder-method-traverseSearch' class='name expandable'>traverseSearch</a>( <span class='pre'>el</span> )<span class=\"signature\"></span></div><div class='description'><div class='short'>Traverses the DOM tree towards root, checking all ancestors\nwith lookup rules, avoiding duplicates. ...</div><div class='long'><p>Traverses the DOM tree towards root, checking all ancestors\nwith lookup rules, avoiding duplicates. Stores positive relations\nin the <a href=\"#!/api/CKEDITOR.plugins.lineutils.finder-property-relations\" rel=\"CKEDITOR.plugins.lineutils.finder-property-relations\" class=\"docClass\">relations</a> object.</p>\n<h3 class=\"pa\">Parameters</h3><ul><li><span class='pre'>el</span> : <a href=\"#!/api/CKEDITOR.dom.element\" rel=\"CKEDITOR.dom.element\" class=\"docClass\">CKEDITOR.dom.element</a><div class='sub-desc'><p>Element which is the starting point.</p>\n</div></li></ul></div></div></div></div></div></div></div>","meta":{"private":true}});