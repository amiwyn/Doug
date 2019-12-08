export default function ({ store, redirect }) {
  if (!store.state.login.authenticated) {
    return redirect('/login')
  }
}