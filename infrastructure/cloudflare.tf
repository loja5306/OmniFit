data "cloudflare_zone" "main" {
  filter = {
    name = "lukeatkinson.dev"
  }
}

resource "cloudflare_dns_record" "cert_validation" {
  zone_id = data.cloudflare_zone.main.zone_id
  name    = tolist(aws_acm_certificate.frontend.domain_validation_options)[0].resource_record_name
  type    = "CNAME"
  content = tolist(aws_acm_certificate.frontend.domain_validation_options)[0].resource_record_value
  proxied = false
  ttl     = 1
}

resource "cloudflare_dns_record" "main" {
  zone_id = data.cloudflare_zone.main.zone_id
  name = var.frontend_domain
  type = "CNAME"
  content = aws_cloudfront_distribution.frontend.domain_name
  proxied = false
  ttl = 1
}