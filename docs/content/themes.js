
var themes = {
    "light" : {
        "button-text" : "Swap to Dark",
        "button-classes" : "btn btn-dark border-light",
        "next-theme" : "dark",
        "body-class" : "bootstrap"
    },
    "dark" : {
        "button-text" : "Swap to Light",
        "button-classes" : "btn btn-light",
        "next-theme" : "light",
        "body-class" : "bootstrap-dark"
    }
};

var themeStorageKey = 'theme';

function swapThemeInDom(theme) {
    var newTheme = themes[theme];
    var bootstrapCSS = document.getElementsByTagName('body')[0];
    bootstrapCSS.setAttribute('class', newTheme['body-class'])
}

function persistNewTheme(theme) {
    window.localStorage.setItem(themeStorageKey, theme);
}

function setToggleButton(theme) {
    var newTheme = themes[theme];
    var themeToggleButton = document.getElementById('theme-toggle');
    themeToggleButton.textContent = newTheme['button-text'];
    themeToggleButton.className = newTheme['button-classes'];
    themeToggleButton.onclick = function() {
        setTheme(newTheme['next-theme']);
    }
}

function setTheme(theme) {
    try {
        swapThemeInDom(theme);
    }
    catch(e){
    }
    try {
    persistNewTheme(theme);
    }
    catch(e) {
    }
    try {
        setToggleButton(theme);
    }
    catch (e) {
    }
}

function getThemeFromStorage() {
    return window.localStorage.getItem(themeStorageKey);
}

function getThemeFromScheme() {
    try {
        if (window.matchMedia("(prefers-color-scheme: dark)").matches){
            return 'dark';
        }
        else {
            return 'light';
        }
    }
    catch(e) {
        return null;
    }
}

function loadTheme() {
    var theme = getThemeFromStorage() || getThemeFromScheme() || 'light';
    setTheme(theme);
}

document.addEventListener('readystatechange', (event) => {
    loadTheme()
});
