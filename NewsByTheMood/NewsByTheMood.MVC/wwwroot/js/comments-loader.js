document.addEventListener("DOMContentLoaded", async function () {
    let Page = 1;
    let isLoading = false;
    let commetsIsOver = false;

    async function loadComments() {
        if (isLoading || commetsIsOver) {
            return;
        }

        try {
            isLoading = true;
            const response = await fetch('/getcomments', {
                method: 'POST',
                body: JSON.stringify({ Page }),
                headers: {
                    'Content-Type': 'application/json',
                    'X-Requested-With': 'XMLHttpRequest',
                },
            });

            if (response.status === 200) {
                const data = await response.text();
                document.getElementById("userComments").innerHTML += data;
                Page++;
   
                isLoading = false;
                return;
            }

            if (response.status === 204) {
                commetsIsOver = true;
                isLoading = false;
                return;
            }

            if (!response.ok) {
                console.error("Error loading comments:" + response.status);
                isLoading = false;
                return;
            }
        }
        catch (error) {
            console.error("Error loading comments:", error);
            isLoading = false;
        }
    }

    // Load comments when the page is scrolled to the bottom
    window.addEventListener("scroll", function () {
        if (window.scrollY + window.innerHeight >= document.body.offsetHeight) {
            loadComments();
        }
    });

    loadComments();
});