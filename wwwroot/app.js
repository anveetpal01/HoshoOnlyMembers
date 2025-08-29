const API_URL = "http://localhost:5002/api";

function saveAuth(token, memberId, name) {
    localStorage.setItem("authToken", token);
    localStorage.setItem("memberId", memberId);
    localStorage.setItem("memberName", name);
}

function getAuth() {
    return {
        token: localStorage.getItem("authToken"),
        memberId: localStorage.getItem("memberId"),
        name: localStorage.getItem("memberName")
    };
}

function logout() {
    localStorage.clear();
    window.location.href = "index.html";
}

// ========== REGISTER ==========
if (document.getElementById("registerForm")) {
    const regForm = document.getElementById("registerForm");
    const otpForm = document.getElementById("otpForm");

    regForm.addEventListener("submit", async (e) => {
        e.preventDefault();
        const payload = {
            name: document.getElementById("regName").value.trim(),
            email: document.getElementById("regEmail").value.trim(),
            mobile: document.getElementById("regMobile").value.trim(),
            password: document.getElementById("regPassword").value
        };

        const res = await fetch(`${API_URL}/member/register`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(payload)
        });

        if (res.ok) {
            const data = await res.json();
            alert("MemberId: " + data.memberId + "\nOTP is: " + data.otp);
            localStorage.setItem("tempMemberId", data.memberId);
            regForm.classList.add("d-none");
            otpForm.classList.remove("d-none");
        } else {
            alert(await res.text());
        }
    });

    otpForm.addEventListener("submit", async (e) => {
        e.preventDefault();
        const memberId = localStorage.getItem("tempMemberId");
        const otp = document.getElementById("otpInput").value;

        const res = await fetch(`${API_URL}/member/verify`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ memberId, otp })
        });

        if (res.ok) {
            alert("OTP Verified! Please login.");
            window.location.reload();
        } else {
            alert(await res.text());
        }
    });
}

// ========== LOGIN ==========
if (document.getElementById("loginForm")) {
    document.getElementById("loginForm").addEventListener("submit", async (e) => {
        e.preventDefault();
        const payload = {
            name: document.getElementById("loginName").value.trim(),
            password: document.getElementById("loginPassword").value
        };

        const res = await fetch(`${API_URL}/member/login`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(payload)
        });

        if (res.ok) {
            const data = await res.json();
            saveAuth(data.token, data.id, data.name);
            window.location.href = "dashboard.html";
        } else {
            alert(await res.text());
        }
    });
}

// ========== DASHBOARD ==========
if (window.location.pathname.endsWith("dashboard.html")) {
    const auth = getAuth();
    if (!auth.token) {
        window.location.href = "index.html";
    } else {
        document.getElementById("userName").innerText = auth.name;

        // Fetch points
        async function loadPoints() {
            const res = await fetch(`${API_URL}/points/${auth.memberId}`, {
                headers: { "Authorization": "Bearer " + auth.token }
            });
            const data = await res.json();
            document.getElementById("totalPoints").innerText = data.totalPoints;
        }

        loadPoints();

        // Add Purchase
        document.getElementById("purchaseForm").addEventListener("submit", async (e) => {
            e.preventDefault();
            const amount = parseInt(document.getElementById("purchaseAmount").value);

            const res = await fetch(`${API_URL}/points/add`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": "Bearer " + auth.token
                },
                body: JSON.stringify({ memberId: auth.memberId, purchaseAmount: amount })
            });

            if (res.ok) {
                const data = await res.json();
                alert("Points added! Total: " + data.totalPoints);
                loadPoints();
            } else alert(await res.text());
        });

        // Redeem
        document.getElementById("redeemForm").addEventListener("submit", async (e) => {
            e.preventDefault();
            const points = parseInt(document.getElementById("redeemPoints").value);

            const res = await fetch(`${API_URL}/coupons/redeem`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": "Bearer " + auth.token
                },
                body: JSON.stringify({ memberId: auth.memberId, pointsToRedeem: points })
            });

            if (res.ok) {
                const data = await res.json();
                alert("Coupon Generated: " + data.couponCode);
                loadCoupons();
                loadPoints();
            } else alert(await res.text());
        });

        // Load All Coupons
        async function loadCoupons() {
            const res = await fetch(`${API_URL}/coupons/${auth.memberId}`, {
                headers: { "Authorization": "Bearer " + auth.token }
            });
            const data = await res.json();
            let html = "<h6>Your Coupons:</h6><ul class='list-group'>";
            data.forEach(c => {
                html += `<li class='list-group-item'>${c.couponCode} - ₹${c.valueInRupees}</li>`;
            });
            html += "</ul>";
            document.getElementById("couponList").innerHTML = html;
        }

        loadCoupons();
    }
}
