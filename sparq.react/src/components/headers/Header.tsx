import styles from "./Header.module.css";

export function Header() {
  return (
    <header className={styles.header}>
      <div className={styles.inner}>
        <h1 className={styles.title}>SparQ</h1>
      </div>
    </header>
  );
}
