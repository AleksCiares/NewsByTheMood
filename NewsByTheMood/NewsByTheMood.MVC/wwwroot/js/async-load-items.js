document.addEventListener("DOMContentLoaded", async function () {
    const loadProcess = {
        Page: 1,
        isLoading: false,
        itemsIsOver: false,
    }

    const loader = document.getElementById("asyncLoadItemsLoader");
    if (!loader) {
        console.error("Loader element not found");
        return;
    }

    const itemsContainer = document.getElementById("asyncLoadItemsContainer");
    if (!itemsContainer) {
        console.error("Items container not found");
        return;
    }

    loader.addEventListener("click", async function () {
        if (loadProcess.isLoading || loadProcess.itemsIsOver) {
            return;
        }

        const result = await loadItems(loadProcess, loader, itemsContainer);

        switch (result) {
            case 200:
                /*this.style.display = "block";*/
                break;
            case 204:
                this.style.display = "none";
                break;
            default:
                this.style.display = "none";
                break;
        }
    });

    loader.click();
});

async function loadItems(process, loader, itemsContainer) {
    if (process.isLoading) {
        return 100;
    }

    try {
        process.isLoading = true;
        setLoaderAnimation(true, loader);

        const Page = process.Page || 1;
        const response = await fetch(asyncLoadUrl, {
            method: asyncLoadMethod,
            body: JSON.stringify({ Page }),
            headers: {
                'Content-Type': 'application/json',
                'X-Requested-With': 'XMLHttpRequest',
            },
        });

        if (response.status === 200) {
            const data = await response.text();
            if (data && data.trim() !== "") {
                itemsContainer.insertAdjacentHTML("beforeend", data);
            }

            process.Page++;
            process.isLoading = false;
            setLoaderAnimation(false, loader);

            return 200;
        }

        if (response.status === 204) {
            console.log("Items is over");

            process.commetsIsOver = true;
            process.isLoading = false;
            setLoaderAnimation(false, loader);

            return 204;
        }

        if (!response.ok) {
            console.error("Error loading items:" + response.status);

            process.isLoading = false;
            setLoaderAnimation(false, loader);

            return response.status;
        }
    }
    catch (error) {
        console.error("Error loading items:", error);

        process.isLoading = false;
        setLoaderAnimation(false, loader);

        return 400;
    }
}

function setLoaderAnimation(isLoading, loader) {
    if (isLoading) {
        loader.classList.add("loading");
        loader.querySelector(".btn-text").textContent = "Loading...";
        loader.querySelector(".spinner-border").classList.remove("d-none");
    }
    else {
        loader.classList.remove("loading");
        loader.querySelector(".btn-text").textContent = "Load";
        loader.querySelector(".spinner-border").classList.add("d-none");
    }
}