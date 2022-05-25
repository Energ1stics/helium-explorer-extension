const api_url = "https://api.helium.io/v1/";

window.onload = () => {
    window.onload = () => {};
    delay(3000).then(async () => {
        var containerElements = document.getElementsByClassName(
            "grid grid-flow-row relative overflow-y-scroll no-scrollbar grid-cols-2 gap-3 md:gap-4 pt-4 px-4 md:pt-8 md:px-8"
        );
        if (containerElements.length !== 1) return;

        for (let index = 1; index < containerElements[0].children.length; ) {
            const element = containerElements[0].children[index];
            element.remove();
        }

        var grid = containerElements[0];

        var clone = grid.firstChild.cloneNode(true);

        grid.appendChild(clone);

        grid.lastElementChild.removeAttribute("href");
        grid.lastElementChild.getElementsByTagName("span")[0].textContent =
            "Sustainable Price";
        await setFairValue(
            grid.lastElementChild.firstChild.childNodes[1],
            30,
            0
        );
        await delay(3000);
        grid.lastElementChild.firstChild.childNodes[2].textContent =
            await getPriceDifference();
        grid.lastElementChild
            .getElementsByTagName("path")[0]
            .setAttribute("d", getGraphPoints());
    });
};

var currentFairValue;

async function setFairValue(textObj, fromDays, toDays) {
    let minted = await mintedTokens(fromDays, toDays);
    let burned = await burnedTokens(fromDays, toDays);

    currentFairValue = calculateFairPrice(burned, minted);
    textObj.innerHTML = "$" + currentFairValue;
}

function calculateFairPrice(fee, mintedToken) {
    return (dcToUSD(fee) / mintedToken).toFixed(3);
}

function dcToUSD(amount) {
    return amount * 0.00001;
}

async function getPriceDifference() {
    console.log("getting price diff");
    let minted = await mintedTokens(60, 30);
    let burned = await burnedTokens(60, 30);

    let pastPrice = calculateFairPrice(burned, minted);
    let difference = currentFairValue - pastPrice;
    var sign = "+";
    if (difference < 0) {
        sign = "-";
        difference = difference * -1;
    }

    return sign + "$" + difference;
}

function getGraphPoints() {
    return "M5,59.75C7.766666666666671,59.08756686891897,10.53333333333334,58.42513373783794,13.300000000000011,57.895711413492265C16.066666666666663,57.3662890891466,18.833333333333314,57.02131278497828,21.599999999999966,56.57346605392597C24.366666666666635,56.12561932287365,27.133333333333308,55.76404396893213,29.899999999999977,55.2086310271784C32.66666666666665,54.65321808542467,35.433333333333316,53.920020584257834,38.19999999999999,53.240988403403584C40.96666666666666,52.561956222549334,43.73333333333333,51.825810205962256,46.5,51.13443794205292C49.26666666666667,50.44306567814358,52.03333333333334,49.981568412738305,54.80000000000001,49.09275481994759C57.566666666666684,48.20394122715687,60.33333333333335,46.65182086908653,63.10000000000002,45.801556385308594C65.86666666666667,44.95129190153067,68.63333333333333,44.52856215368781,71.39999999999998,43.99116791728";
}

async function burnedTokens(fromDays, toDays) {
    var burned;
    await fetch(
        api_url +
            "dc_burns/sum?min_time=-" +
            fromDays +
            "%20day&max_time=-" +
            toDays +
            "%20day"
    )
        .then((res) => res.json())
        .then((data) => (burned = data.data.fee));
    return burned;
}

async function mintedTokens(fromDays, toDays) {
    var minted;
    await fetch(
        api_url +
            "rewards/sum?min_time=-" +
            fromDays +
            "%20day&max_time=-" +
            toDays +
            "%20day"
    )
        .then((res) => res.json())
        .then((data) => (minted = data.data.total));
    return minted;
}

function delay(time) {
    return new Promise((resolve) => {
        console.log("setting timeout");
        setTimeout(resolve, time);
    });
}
