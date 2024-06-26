const api_url = "https://helium-api-h6zi65bsjq-od.a.run.app/api/";

let currentMinValue = getEquilibrium(0);
let lastMonthMinValue = getEquilibrium(30);

window.onload = () => {
    addMinValueElement();
};

function relativeValueChange(v1, v2) {
    if (v1 > v2) {
        return `+${((v1 / v2 - 1) * 100).toFixed(2)}`;
    } else {
        return `-${((v2 / v1 - 1) * 100).toFixed(2)}`;
    }
}

async function addMinValueElement() {
    var elementsContainer = document.getElementsByClassName(
        "grid grid-flow-row relative overflow-y-scroll no-scrollbar grid-cols-2 gap-3 md:gap-4 pt-4 px-4 md:pt-8 md:px-8"
    )[0];

    if (!elementsContainer) return;

    elementsContainer.insertBefore(
        elementsContainer.children[3].cloneNode(true),
        elementsContainer.childNodes[3]
    );

    var minValueElement = elementsContainer.children[4];
    minValueElement.firstChild.firstChild.firstChild.textContent =
        "Equilibrium of HNT (30d)";
    minValueElement.firstChild.firstChild.lastChild.remove();
    minValueElement.firstChild.childNodes[1].firstChild.firstChild.textContent =
        "Loading...";
    minValueElement.firstChild.childNodes[2].textContent = "Loading...";

    currentMinValue = await currentMinValue;

    minValueElement.firstChild.childNodes[1].firstChild.firstChild.textContent = `$${
        currentMinValue / 100
    }`;

    lastMonthMinValue = await lastMonthMinValue;

    let relValChange = relativeValueChange(currentMinValue, lastMonthMinValue);

    if (relValChange < 0) {
        minValueElement
            .getElementsByClassName("text-sm font-medium")[0]
            .classList.add("text-navy-400");
    } else {
        minValueElement
            .getElementsByClassName("text-sm font-medium")[0]
            .classList.add("text-green-500");
    }

    minValueElement.getElementsByClassName(
        "text-sm font-medium"
    )[0].textContent = relValChange + "%";
}

async function getEquilibrium(minusDays) {
    let date = new Date();
    date.setDate(date.getDate() - minusDays);
    const year = date.getUTCFullYear();
    const month = date.getUTCMonth() + 1;
    const day = date.getUTCDate();
    const uri = `${api_url}DailyStats/${year}/${month}/${day}/30`;

    return getData(uri);
}

async function getData(uri) {
    let tries = 0;
    var maxTries = 30;

    while (tries != maxTries) {
        tries++;
        let data = await fetch(uri)
            .then((response) => {
                if (response.ok) {
                    return response.json();
                } else if (response.status === 503) {
                    return null;
                } else {
                    console.log(response.statusText);
                }
            })
            .then((data) => {
                return data;
            })
            .catch((error) => console.error(error));
        if (data) {
            return data;
        }
        await sleep(1000);
    }
    console.log("No response from API");
}

function sleep(milliseconds) {
    return new Promise((resolve) => setTimeout(resolve, milliseconds));
}
