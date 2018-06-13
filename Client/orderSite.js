var mlCodes = [
    {
        code: "en",
        name: "English",
    },
    {
        code: "da",
        name: "Danish",
    },
  ];

var MLStrings = [
    {
        English: "Hello you",
        Dansih: "Hej dig"
    }
];
var mlrLangInUse;
var mlr = function(_a){
    var _b = _a === void 0 ? {} : _a, _c = _b.dropID, dropID = _c === void 0 ? "languageDropMenu" : _c, _d = _b.stringAttribute, stringAttribute = _d === void 0 ? "data-mlr-text" : _d, _e = _b.chosenLang, chosenLang = _e === void 0 ? "English" : _e, _f = _b.mLstrings, mLstrings = _f === void 0 ? MLStrings: _f, _g = _b.countryCodes, countryCodes = _g === void 0 ? false : _g, _h = _b.countryCodeData, countryCodeData = _h === void 0 ? [] : _h;
    var root = document.documentElement;
    var listOfLanguages = Object.keys(mLstrings[0]);
    mlrLangInUse = chosenLang;
    (function createMLDrop(){
        console.log(dropID)
        var languageDropMenu = document.getElementById(dropID);
        languageDropMenu.innerHTML = "";
        listOfLanguages.forEach(function(lang, langidx){
            var HTMLoption = document.createElement("option");
            HTMLoption.value = lang;
            HTMLoption.textContent = lang;
            languageDropMenu.appendChild(HTMLoption);
            if(lang === chosenLang){
                languageDropMenu.value = lang
            }
        });
        languageDropMenu.addEventListener("change", function(e){
            mlrLangInUse = languageDropMenu[languageDropMenu.selectedIndex].value;
            resolveAllMLStrings();
            if(countryCodes === true){
                if (!Array.isArray(countryCodeData) || !countryCodeData.length){
                    console.warn("Cannot access language packet");
                    return;
                }
                root.setAttribute("lang", updateCountryCodeOnHTML().code);
            }
        });
    });
    function updateCountryCodeOnHTML() {
        return countryCodeData.find(function(this2Digit){ return this2Digit.name === mlrLangInUse; });
    }

    function resolveAllMLStrings(){
        var stringsToBeResolved = document.querySelectorAll("["+ stringAttribute +"]");
        stringsToBeResolved.forEach(function (stringsToBeResolved){
            var originalTextContent = stringsToBeResolved.textContent;
            var resolvedText = resolveMLString(originalTextContent, mLstrings);
            stringsToBeResolved.textContent = resolvedText;
        });
    }
};
function resolveMLString(stringsToBeResolved,mLstrings) {
    var matchingStringIndex = mLstrings.find(function(stringObj){
        var stringValues = Object.values(stringObj);
        return stringValues.includes(stringsToBeResolved);
    });
    if(matchingStringIndex){
        return matchingStringIndex[mlrLangInUse];
    } else {
        return stringsToBeResolved
    }
}
mlr({
    dropID: "languageDropMenu",
    stringAttribute: "data-mlr-text",
    chosenLang: "English",
    mLstrings: MLStrings,
    countryCodes: true,
    countryCodeData: mlCodes
});


    