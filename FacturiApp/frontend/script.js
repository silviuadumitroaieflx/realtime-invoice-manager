const API = "http://localhost:5279";

const { createApp } = Vue;

createApp({
  data() {
    return {
      token: localStorage.getItem("token") || "",
      username: localStorage.getItem("username") || "",
      authMode: "login",
      authMesaj: "",
      loginForm: { username: "", password: "" },
      registerForm: { username: "", nume: "", email: "", password: "" },
      resetForm: { username: "", email: "", nouaParola: "" },
      drawerOpen: false,

      pagina: "produse",
      mesaj: "",
      produse: [],
      clienti: [],
      facturi: [],
      plati: [],
      produsNou: { numeProdus: "", pretUnitar: 0, unitateMasura: "" },
      clientNou: { numeClient: "", email: "", telefon: "", adresa: "" },
      facturaNoua: { idClient: 0, observatii: "", produseFactura: [] },
      produsSelectat: 0,
      cantitateSelectata: 1,
      filtruClient: 0,
      plataNoua: { nrFactura: 0, suma: 0, metodaPlata: "numerar", status: "Platita" },
    };
  },

  computed: {
    autentificat() {
      return !!this.token;
    },
    totalFactura() {
      return this.facturaNoua.produseFactura
        .reduce((s, l) => s + l.pretUnitar * l.cantitate, 0);
    }
  },

  methods: {
    headere() {
      const h = { "Content-Type": "application/json" };
      if (this.token) h["Authorization"] = "Bearer " + this.token;
      return h;
    },

    schimbaMod(mod) {
      this.authMode = mod;
      this.authMesaj = "";
    },

    async login() {
      const r = await fetch(`${API}/Auth/Login`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(this.loginForm),
      });
      if (!r.ok) { this.authMesaj = "Username sau parola gresita."; return; }

      const data = await r.json();
      this.token = data.token;
      this.username = this.loginForm.username;
      localStorage.setItem("token", this.token);
      localStorage.setItem("username", this.username);
      this.loginForm = { username: "", password: "" };
      this.incarcaTot();
    },

    async register() {
      const r = await fetch(`${API}/Auth/Register`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(this.registerForm),
      });
      if (!r.ok) {
        const txt = await r.text();
        this.authMesaj = txt || "Eroare la inregistrare.";
        return;
      }
      this.authMesaj = "Cont creat! Acum te poti autentifica.";
      this.authMode = "login";
      this.registerForm = { username: "", nume: "", email: "", password: "" };
    },

    async resetParola() {
      const r = await fetch(`${API}/Auth/ResetPassword`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(this.resetForm),
      });
      const txt = await r.text();
      if (!r.ok) { this.authMesaj = txt || "Date gresite."; return; }
      this.authMesaj = "Parola schimbata! Acum te poti autentifica.";
      this.authMode = "login";
      this.resetForm = { username: "", email: "", nouaParola: "" };
    },

    logout() {
      this.token = "";
      this.username = "";
      localStorage.removeItem("token");
      localStorage.removeItem("username");
      this.drawerOpen = false;
    },

    incarcaTot() {
      this.incarcaProduse();
      this.incarcaClienti();
      this.incarcaFacturi();
    },

    conecteazaLive() {
      const conn = new signalR.HubConnectionBuilder()
        .withUrl(`${API}/clientHub`)
        .build();

      conn.on("ClientiModificati", (info) => {
        this.mesaj = "Live: s-a " + info;
        this.incarcaClienti();
      });

      conn.start();
    },

    async incarcaProduse() {
      const r = await fetch(`${API}/Produs/GetProduse`, { headers: this.headere() });
      this.produse = await r.json();
    },
    async adaugaProdus() {
      await fetch(`${API}/Produs/PostProdus`, {
        method: "POST",
        headers: this.headere(),
        body: JSON.stringify(this.produsNou),
      });
      this.mesaj = "Produs adaugat!";
      this.produsNou = { numeProdus: "", pretUnitar: 0, unitateMasura: "" };
      this.incarcaProduse();
    },

    async incarcaClienti() {
      const r = await fetch(`${API}/Client/GetClienti`, { headers: this.headere() });
      this.clienti = await r.json();
    },
    async adaugaClient() {
      await fetch(`${API}/Client/PostClient`, {
        method: "POST",
        headers: this.headere(),
        body: JSON.stringify(this.clientNou),
      });
      this.mesaj = "Client adaugat!";
      this.clientNou = { numeClient: "", email: "", telefon: "", adresa: "" };
      this.incarcaClienti();
    },
    async stergeClient(id) {
      await fetch(`${API}/Client/DeleteClient?id=${id}`, {
        method: "DELETE",
        headers: this.headere(),
      });
      this.mesaj = "Client sters!";
      this.incarcaClienti();
    },

    adaugaLinie() {
      const p = this.produse.find(x => x.idProdus === this.produsSelectat);
      if (!p) { this.mesaj = "Alege un produs!"; return; }
      this.facturaNoua.produseFactura.push({
        idProdus: p.idProdus,
        denumire: p.numeProdus,
        pretUnitar: p.pretUnitar,
        cantitate: this.cantitateSelectata,
      });
      this.cantitateSelectata = 1;
    },
    async creeazaFactura() {
      await fetch(`${API}/Factura/AdaugaFactura`, {
        method: "POST",
        headers: this.headere(),
        body: JSON.stringify(this.facturaNoua),
      });
      this.mesaj = "Factura creata!";
      this.facturaNoua = { idClient: 0, observatii: "", produseFactura: [] };
      this.incarcaFacturi();
    },
    async incarcaFacturi() {
      const r = await fetch(`${API}/Factura/GetFacturi`, { headers: this.headere() });
      this.facturi = await r.json();
    },
    async incarcaFacturiClient() {
      if (this.filtruClient === 0) return;
      const r = await fetch(`${API}/Factura/GetFacturiByClient?idClient=${this.filtruClient}`, { headers: this.headere() });
      this.facturi = r.ok ? await r.json() : [];
    },

    totalFacturii(f) {
      return (f.produseFactura || [])
        .reduce((s, l) => s + l.pretUnitar * l.cantitate, 0);
    },

    clientPlatii(pl) {
      const f = this.facturi.find(x => x.nrFactura === pl.nrFactura);
      if (f && f.client) return f.client.numeClient;
      return "(factura " + pl.nrFactura + ")";
    },

    async incarcaPlati() {
      const r = await fetch(`${API}/Plata/GetPlati`, { headers: this.headere() });
      this.plati = await r.json();
    },
    async adaugaPlata() {
      const r = await fetch(`${API}/Plata/PostPlata`, {
        method: "POST",
        headers: this.headere(),
        body: JSON.stringify(this.plataNoua),
      });
      this.mesaj = r.ok ? "Plata inregistrata!" : "Eroare: factura nu exista?";
      this.incarcaPlati();
    },
  },

  mounted() {
    if (this.autentificat) {
      this.incarcaTot();
      this.conecteazaLive();
    }
  },
}).mount("#app");
