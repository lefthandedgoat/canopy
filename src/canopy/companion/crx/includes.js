    requirejs.config({
        baseUrl: 'out',  // Set the baseUrl to the path of the compiled JS code
        paths: {
            // Explicit path to core lib (relative to baseUrl, omit .js)
            'fable-core': '../fable-core/'
        }
    });
    requirejs(["companion.js"]);