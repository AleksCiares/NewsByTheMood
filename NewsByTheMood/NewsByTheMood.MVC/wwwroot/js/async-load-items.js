document.addEventListener("DOMContentLoaded", async function () {
    const loadProcess = {
        Page: 1,
        isLoading: false,
        itemsIsOver: false,
    }

    document.getElementById("asyncLoadItemsLoader").addEventListener("click", async function () {
        if (loadProcess.isLoading || loadProcess.itemsIsOver) {
            return;
        }
        /*if (loadButton.classList.contains("loading")) {
            return;
        }*/

        this.classList.add("loading");
        this.querySelector(".btn-text").textContent = "Loading...";
        this.querySelector(".spinner-border").classList.remove("d-none");

        const result = await loadItems(loadProcess);

        this.classList.remove("loading");
        this.querySelector(".btn-text").textContent = "Load";
        this.querySelector(".spinner-border").classList.add("d-none");

        switch (result) {
            case 200:
                this.style.display = "block";
                break;
            case 204:
                this.style.display = "none";
                break;
            default:
                this.style.display = "none";
                break;
        }
    });

    await loadItems(loadProcess);
});

async function loadItems(process) {
    if (process.isLoading) {
        return 100;
    }

    try {
        process.isLoading = true;
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
            document.getElementById("asyncLoadItemsContainer").insertAdjacentHTML("beforeend", data);
            process.Page++;
            process.isLoading = false;

            return 200;
        }

        if (response.status === 204) {
            console.log("Items is over");
            process.commetsIsOver = true;
            process.isLoading = false;

            return 204;
        }

        if (!response.ok) {
            console.error("Error loading items:" + response.status);
            process.isLoading = false;

            return response.status;
        }
    }
    catch (error) {
        console.error("Error loading items:", error);
        process.isLoading = false;

        return 400;
    }
}